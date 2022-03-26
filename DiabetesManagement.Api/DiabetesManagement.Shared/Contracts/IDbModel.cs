﻿using System.Reflection;

namespace DiabetesManagement.Shared.Contracts
{
    public interface IDbModel
    {
        string IdProperty { get; }
        IEnumerable<string> Columns { get; }
        string TableName { get; }
        string ColumnDelimitedList { get; }
        string WhereClause { get; }
        string ResolveColumnName(PropertyInfo property, bool fullyQualified);
    }
}
