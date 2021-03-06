using System.Reflection;

namespace DiabetesManagement.Shared.Contracts
{
    public interface IDbModel
    {
        IEnumerable<PropertyInfo> Properties { get; }
        string FullyQualifiedColumnDelimitedListWithAlias { get; }
        string IdProperty { get; }
        IEnumerable<string> Columns { get; }
        string TableName { get; }
        string FullyQualifiedColumnDelimitedList { get; }
        string ColumnDelimitedList { get; }
        string WhereClause { get; }
        IQueryBuilder QueryBuilder { get; internal set; }
        string ResolveColumnName(PropertyInfo property, bool fullyQualified);
        string ResolveColumnName(string propertyName, bool fullyQualified);
    }
}
