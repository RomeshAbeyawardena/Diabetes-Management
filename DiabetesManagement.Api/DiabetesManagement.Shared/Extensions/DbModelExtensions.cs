using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Shared.Extensions
{
    public static class DbModelExtensions
    {
        public static string Build(this IDbModel model, int? topAmount = null, string? whereClause = default)
        {
            var query = "SELECT ";

            if (topAmount.HasValue)
            {
                query += $"TOP({topAmount}) ";
            }

            return query += $"{model.ColumnDelimitedList} FROM {model.TableName} " + whereClause;
        }
    }
}
