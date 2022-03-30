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
            return new DefaultQueryBuilder<TModel>(model, joinDefinitions);
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
                Debug.WriteLine("{0} {1}:{2}", nameof(ToDynamic), property, val);
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
            var query = string.Empty; //model.Build(BuildMode.Insert);
            Debug.WriteLine(query, nameof(Insert));
            var d = ToDynamic(model);
            return await dbConnection.ExecuteScalarAsync<Guid>(query, d, transaction);
        }

        public static async Task<Guid> Update<TRequest>(this IDbModel model, 
            TRequest request, 
            IDbConnection dbConnection, 
            IDbTransaction? transaction)
        {
            var query = string.Empty; //model.Build(BuildMode.Update, request);
            Debug.WriteLine(query, nameof(Update));
            var d = ToDynamic(model);
            return await dbConnection.ExecuteScalarAsync<Guid>(query, d, transaction);
        }
    }
}
