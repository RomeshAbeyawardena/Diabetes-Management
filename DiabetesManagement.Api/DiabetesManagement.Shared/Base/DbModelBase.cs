using DiabetesManagement.Shared.Contracts;
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
        private TableAttribute? tableAttribute;
        private IEnumerable<string>? columns;

        protected virtual IEnumerable<string> Columns => columns ??= GetColumns();
        protected virtual Type EntityType => type ??= GetType();
        protected virtual IEnumerable<PropertyInfo> Properties => properties ??= EntityType.GetProperties();
        protected virtual string TableName => tableName ??= (tableAttribute = EntityType.GetCustomAttribute<TableAttribute>())?.Name ?? EntityType.Name;
        protected virtual string Schema => schema ??= tableAttribute?.Schema ?? "dbo";

        protected IEnumerable<string> GetColumns()
        {
            var columnsList = new List<string>();
            foreach(var property in Properties)
            {
                if (!property.CanWrite || property.Name == nameof(IdProperty))
                {
                    continue;
                }

                var propertyType = property.PropertyType;
                var columnAttribute = propertyType.GetCustomAttribute<ColumnAttribute>();
                var keyAttribute = propertyType.GetCustomAttribute<KeyAttribute>();

                var name = columnAttribute != null ? columnAttribute.Name : property.Name;

                if(keyAttribute != null)
                {
                    IdProperty = name;
                }

                columnsList.Add(name!);
            }

            return columnsList;
        }

        [AllowNull] public string IdProperty { get; protected set; }

        IEnumerable<string> IDbModel.Columns => Columns;

        string IDbModel.TableName => TableName;

        public string ColumnDelimitedList => $"{string.Join(",", Columns)}";
        public string WhereClause => $"WHERE [{IdProperty}]= @id";
    }
}
