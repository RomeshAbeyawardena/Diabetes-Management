using DiabetesManagement.Shared.Contracts;
using DiabetesManagement.Shared.Defaults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DiabetesManagement.Shared.Base
{
    public abstract class DbModelBase : IDbModel
    {
        private string? schema;
        private string? tableName;
        private Type? type;
        private IEnumerable<PropertyInfo>? properties;
        private readonly IDictionary<PropertyInfo, string> columnResolutionDictionary;
        private TableAttribute? tableAttribute;
        private IEnumerable<string>? columns;

        private void UpdateModelCache()
        {
            var currentModelCache = DefaultDbModelCache.Current;
            //            currentModelCache.TryAdd(EntityType, this);
        }

        protected virtual TableAttribute? TableAttribute => tableAttribute ??= EntityType.GetCustomAttribute<TableAttribute>();
        protected virtual IEnumerable<string> Columns
        {
            get
            {
                if (columns == null)
                {
                    columns = GetColumns();
                }

                return columns;
            }
        }

        protected virtual Type EntityType => type ??= GetType();
        protected virtual string TableName => tableName ??= TableAttribute?.Name ?? EntityType.Name;
        protected virtual string Schema => schema ??= TableAttribute?.Schema ?? "dbo";

        protected IEnumerable<string> GetColumns()
        {
            var columnsList = new List<string>();
            foreach (var property in Properties)
            {
                if (!property.CanWrite || property.Name == nameof(IdProperty))
                {
                    continue;
                }

                var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                var keyAttribute = property.GetCustomAttribute<KeyAttribute>();

                var name = columnAttribute != null ? columnAttribute.Name : property.Name;

                if (keyAttribute != null)
                {
                    IdProperty = property.Name;
                }

                columnResolutionDictionary.TryAdd(property, name!);

                columnsList.Add(name!);
            }

            return columnsList;
        }

        public DbModelBase()
        {
            columnResolutionDictionary = new Dictionary<PropertyInfo, string>();
        }

        public string ResolveColumnName(string propertyName, bool fullyQualified)
        {
            if (!columnResolutionDictionary.Any())
            {
                GetColumns();
            }

            var kv = columnResolutionDictionary.FirstOrDefault(c => c.Key.Name == propertyName || c.Value == propertyName);

            if (kv.Equals(default(KeyValuePair<PropertyInfo, string>)))
            {
                return string.Empty;
            }

            return ResolveColumnName(kv.Key!, fullyQualified);
        }

        public string ResolveColumnName(PropertyInfo property, bool fullyQualified)
        {
            if (property == null)
            {
                return null!;
            }

            if (!columnResolutionDictionary.Any())
            {
                GetColumns();
            }

            if (columnResolutionDictionary.TryGetValue(property, out var columnName))
            {
                return fullyQualified ? $"[{TableName}].[{columnName}]" : $"[{columnName}]";
            }

            return string.Empty;
        }

        [AllowNull] public string IdProperty { get; protected set; }

        IEnumerable<string> IDbModel.Columns => Columns;

        string IDbModel.TableName => $"[{Schema}].[{TableName}]";
        public virtual IEnumerable<PropertyInfo> Properties => properties ??= EntityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        public string FullyQualifiedColumnDelimitedList => $"[{TableName}].[{string.Join($"], [{TableName}].[", Columns)}]";
        
        public string FullyQualifiedColumnDelimitedListWithAlias 
        {
            get {
                if (!columnResolutionDictionary.Any())
                {
                    GetColumns();
                }

                var items = new List<string>();

                foreach(var (property, column) in columnResolutionDictionary)
                {
                    items.Add($"[{TableName}].[{column}] [{property.Name}]");
                }

                return string.Join(", ", items);
            }
        }

        public string ColumnDelimitedList => $"[{string.Join("],[", Columns)}]";
        public virtual string WhereClause => $"WHERE [{IdProperty}]= @{IdProperty}";

        IQueryBuilder IDbModel.QueryBuilder { get; set; }
    }
}
