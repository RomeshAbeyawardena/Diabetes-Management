using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Shared.Extensions
{
    public static class DbModelExtensions
    {
        public static string Build(this IDbModel model, int? topAmount = null, string? whereClause = default, Action<IJoinDefinition>? action = null)
        {
            var query = "SELECT ";

            if (topAmount.HasValue)
            {
                query += $"TOP({topAmount}) ";
            }

            return query += $"{model.ColumnDelimitedList} FROM {model.TableName} " + whereClause;
        }

        public static IJoinDefinitionBuilder JoinDefinitionsBuilder(this IDbModel model, Action<IJoinDefinitionBuilder> builder)
        {
            var joinDefinitionBuilder = new JoinDefinitionBuilder();

            builder?.Invoke(joinDefinitionBuilder);

            return joinDefinitionBuilder;
        }
    }
}
