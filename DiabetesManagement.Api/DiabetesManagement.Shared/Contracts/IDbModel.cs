namespace DiabetesManagement.Shared.Contracts
{
    public interface IDbModel
    {
        public string IdProperty { get; }
        public IEnumerable<string> Columns { get; }
        public string TableName { get; }
        public string ColumnDelimitedList { get; }
        public string WhereClause { get; }
    }
}
