using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace DiabetesManagement.Shared.Extensions
{
    using DiabetesManagement.Shared.Contracts;
    using DiabetesManagement.Shared.Defaults;
    using DiabetesManagement.Shared.Enumerations;
    using System.Diagnostics;
    using System.Dynamic;
    using System.Runtime.Serialization;

    public static class DbModelExtensions
    {
        public static IChangeSet<TSource, TDestination> GetChangeSet<TSource, TDestination>(this TDestination model, TSource source)
            where TDestination : IDbModel
        {
            var changeSetDetector = new DefaultChangeSetDetector(model.Properties);
            return changeSetDetector.DetectChanges(source, model);
        }

        public static string Build<TRequest>(
            this IDbModel model, 
            BuildMode buildMode, 
            TRequest request)
        {
            if (buildMode == BuildMode.Update)
            {
                var query = $"UPDATE {model.TableName} SET ";

                query += GenerateWhereClause(model, request, ", ");

                return $"{query} {model.WhereClause}";
            }

            throw new NotSupportedException();
        }
        
        public static string Build(this IDbModel model, BuildMode buildMode)
        {
            var columnsDelimitedList = model.FullyQualifiedColumnDelimitedList;

            if (buildMode == BuildMode.Insert)
            {
                var query = $"INSERT INTO {model.TableName} ({columnsDelimitedList}) VALUES (";
                query += $"@{string.Join(", @", model.Properties.Select(p => p.Name))}";
                return query += $"); SELECT @{model.IdProperty}";
            }

            throw new NotSupportedException();
        }

        public static string Build(this IDbModel model, 
            int? topAmount = null, 
            string? whereClause = default, 
            Action<IJoinDefinitionBuilder>? builder = null)
        {
            string GetWhereClause()
            {
                return !string.IsNullOrWhiteSpace(whereClause) ? $"WHERE {whereClause}" : string.Empty;
            }

            var query = "SELECT ";
            var otherColumns = string.Empty;
            string GetQuery()
            {
                return query.Replace("@@otherColumns", !string.IsNullOrEmpty(otherColumns) ? $", {otherColumns}" : string.Empty);
            }

            if (topAmount.HasValue)
            {
                query += $"TOP({topAmount}) ";
            }

            query += model.FullyQualifiedColumnDelimitedList + "@@otherColumns";
            
            var joinDefinitionBuilder = model.JoinDefinitionsBuilder(builder!);
            if (joinDefinitionBuilder.Any())
            {
                query += $" {joinDefinitionBuilder.Build(out string otherCols)}";

                otherColumns += otherCols;
                return $"{GetQuery()} {GetWhereClause()} ";
            }

            return $"{GetQuery()} FROM {model.TableName} {GetWhereClause()}";
        }

        public static IJoinDefinitionBuilder JoinDefinitionsBuilder(this IDbModel model, Action<IJoinDefinitionBuilder> builder)
        {
            var joinDefinitionBuilder = new DefaultJoinDefinitionBuilder();

            builder?.Invoke(joinDefinitionBuilder);

            return joinDefinitionBuilder;
        }

        public static string GenerateWhereClause<TRequest>(this IDbModel model, TRequest request, string defaultLogicalOperator = " AND ")
        {
            string query = string.Empty;

            foreach (var property in typeof(TRequest).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance))
            {
                var ignoreDataMemberAttribute = property.GetCustomAttribute<IgnoreDataMemberAttribute>();

                if (ignoreDataMemberAttribute != null)
                {
                    continue;
                }

                var propertyValue = property.GetValue(request);

                var defaultValue = property.PropertyType.GetDefaultValue();
                if (propertyValue == null || propertyValue.Equals(defaultValue))
                {
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(query))
                {
                    query += defaultLogicalOperator;
                }

                var name = property.Name;
                var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();

                if (columnAttribute != null)
                {
                    name = columnAttribute.Name;
                }

                var columnName = model.ResolveColumnName(name!, true);

                query += $"{columnName} = @{property.Name}";
            }

            return query;
        }

        public static async Task<IEnumerable<TResponse>> Get<TRequest, TResponse>(this TResponse model,
            IDbConnection dbConnection,
            TRequest request, int topAmount = 1,
            string? orderByQuery = default,
            Action<IJoinDefinitionBuilder>? builder = null,
            IDbTransaction? transaction = null)
            where TResponse : class, IDbModel
        {
            return await Get<TRequest, TResponse>((IDbModel)model, dbConnection, request, topAmount, orderByQuery, builder, transaction);
        }

        public static async Task<IEnumerable<TResponse>> Get<TRequest, TResponse>(this IDbModel model,
            IDbConnection dbConnection,
            TRequest request, int topAmount = 1,
            string? orderByQuery = default,
            Action<IJoinDefinitionBuilder>? builder = null,
            IDbTransaction? transaction = null)
        {
            var query = model.Build(topAmount, GenerateWhereClause(model, request), builder);
            Debug.WriteLine(query, nameof(Get));
            if (!string.IsNullOrWhiteSpace(orderByQuery))
                query = $"{query} {orderByQuery}";
            return await dbConnection.QueryAsync<TResponse>(query, request!.ToDynamic(), transaction);
        }

        public static ExpandoObject ToDynamic(this object value, IEnumerable<PropertyInfo>? properties = null)
        {
            ExpandoObject dynamic = new();

            if(properties == null)
            {
                properties = value.GetType().GetProperties();
            }

            foreach (var property in properties)
            {
                var ignoreDataMemberAttribute = property.GetCustomAttribute<IgnoreDataMemberAttribute>();
                if (ignoreDataMemberAttribute != null)
                {
                    continue;
                }

                var val = property.GetValue(value);
                dynamic.TryAdd(property.Name, val);
            }

            return dynamic;
        }

        public static ExpandoObject ToDynamic(this IDbModel model)
        {
            return model.ToDynamic(model.Properties);
        }

        public static async Task<Guid> Insert(this IDbModel model, IDbConnection dbConnection, IDbTransaction? transaction)
        {
            var query = model.Build(BuildMode.Insert);
            Debug.WriteLine(query, nameof(Insert));
            var d = ToDynamic(model);
            return await dbConnection.ExecuteScalarAsync<Guid>(query, d, transaction);
        }

        public static async Task<Guid> Update<TRequest>(this IDbModel model, 
            TRequest request, 
            IDbConnection dbConnection, 
            IDbTransaction? transaction)
        {
            var query = model.Build(BuildMode.Update, request);
            Debug.WriteLine(query, nameof(Update));
            var d = ToDynamic(model);
            return await dbConnection.ExecuteScalarAsync<Guid>(query, d, transaction);
        }
    }
}
