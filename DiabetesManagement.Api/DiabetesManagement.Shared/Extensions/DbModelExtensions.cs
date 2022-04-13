using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace DiabetesManagement.Shared.Extensions
{
    using DiabetesManagement.Shared.Contracts;
    using DiabetesManagement.Shared.Defaults;
    using DiabetesManagement.Shared.Enumerations;
    using Humanizer;
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

        public static IJoinDefinitionBuilder JoinDefinitionsBuilder(this IDbModel model, Action<IJoinDefinitionBuilder> builder)
        {
            var joinDefinitionBuilder = new DefaultJoinDefinitionBuilder();

            builder?.Invoke(joinDefinitionBuilder);

            return joinDefinitionBuilder;
        }

        public static IQueryBuilder<TModel> QueryBuilder<TModel>(this TModel model, 
            IJoinDefinitionBuilder? joinDefinitions = default,
            Action<IQueryBuilder<TModel>>? builder = default)
            where TModel : IDbModel
        {
            var queryBuilder = new DefaultQueryBuilder<TModel>(model, joinDefinitions);

            builder?.Invoke(queryBuilder);

            return queryBuilder!;
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
            
            var query = model
                .QueryBuilder(model.JoinDefinitionsBuilder(builder!), 
                    b => b.SetBuildMode(BuildMode.Select).SetTopAmount(topAmount).GenerateWhereClause(request)).Query;
            
            //Debug.WriteLine(query, nameof(Get));
            if (!string.IsNullOrWhiteSpace(orderByQuery))
                query = $"{query} {orderByQuery}";

            return await dbConnection.QueryAsync<TResponse>(query, request!.ToDynamic(), transaction);
        }

        public static ExpandoObject ToDynamic(this object value, IEnumerable<PropertyInfo>? properties = null, bool convertDatesToIsoString = false)
        {
            string FormatDate(DateTimeOffset? dateValue)
            {
                if (dateValue.HasValue)
                {
                    return dateValue.Value.ToString("yyyy-MM-ddTHH:mm:ss.sssZ");
                }

                return string.Empty;
            }

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
                if (convertDatesToIsoString)
                {
                    if (property.PropertyType == typeof(DateTimeOffset) || property.PropertyType == typeof(DateTimeOffset?))
                    {
                        var dateValue = (DateTimeOffset?)val;
                        val = FormatDate(dateValue);
                    }
                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    {
                        var dateValue = (DateTime?)val;
                        val = FormatDate(dateValue);
                    }
                }

                dynamic.TryAdd(property.Name.Camelize(), val);
            }

            return dynamic;
        }

        public static ExpandoObject ToDynamic(this IDbModel model, bool convertDatesToIsoString)
        {
            if (model == null)
                return new ExpandoObject();

            return model.ToDynamic(model.Properties, convertDatesToIsoString);
        }

        public static async Task<Guid> Insert(this IDbModel model, IDbConnection dbConnection, IDbTransaction? transaction)
        {
            var query = model.QueryBuilder(builder: b => b.SetBuildMode(BuildMode.Insert)).Query;
            //Debug.WriteLine(query, nameof(Insert));
            var d = ToDynamic(model);
            return await dbConnection.ExecuteScalarAsync<Guid>(query, d, transaction);
        }

        public static async Task<Guid> Update<TRequest>(this IDbModel model, 
            TRequest request, 
            TRequest whereRequest,
            IDbConnection dbConnection, 
            IDbTransaction? transaction)
        {
            var query = model.QueryBuilder(builder: b => b
                .SetBuildMode(BuildMode.Update)
                .GenerateUpdateBody(request).GenerateWhereClause(whereRequest)).Query; //model.Build(BuildMode.Update, request);
            //Debug.WriteLine(query, nameof(Update));
            var d = ToDynamic(model);
            return await dbConnection.ExecuteScalarAsync<Guid>(query, d, transaction);
        }
    }
}
