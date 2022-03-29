using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace DiabetesManagement.Shared.Defaults
{
    using DiabetesManagement.Shared.Contracts;
    using DiabetesManagement.Shared.Enumerations;
    using DiabetesManagement.Shared.Extensions;

    public class DefaultQueryBuilder<TModel> : IQueryBuilder<TModel>
        where TModel : IDbModel
    {
        private readonly IJoinDefinitionBuilder? joinDefinitions;
        private string? columns;
        private string whereClause = string.Empty;
        private string updateBody = string.Empty;

        private string GenerateWhereClause<TRequest>(TRequest request, string defaultLogicalOperator)
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

                var columnName = Model.ResolveColumnName(name!, true);

                if (string.IsNullOrEmpty(columnName))
                {
                    columnName = ResolveColumn(name!);
                }

                query += $"{columnName} = @{property.Name}";
            }

            return query;
        }

        private static string GetColumns(IEnumerable<IJoinDefinition> joinDefinitions)
        {
            var columnList = new StringBuilder();
            
            void ExtractColumns(IDbModel model)
            {
                if (columnList.Length != 0)
                {
                    columnList.Append(',');
                }

                columnList.Append($"{model.FullyQualifiedColumnDelimitedList}");
            }

            foreach(var joinDefinition in joinDefinitions)
            {
                if (joinDefinition.Parent is IDbModel parent)
                {
                    ExtractColumns(parent);
                }

                if (joinDefinition.Child is IDbModel child)
                {
                    ExtractColumns(child);
                }
            }

            return columnList.ToString();
        }

        private string GenerateQuery()
        {
            IEnumerable<PropertyInfo> properties = Model.Properties;
            var query = new StringBuilder();
            query.Append(GetQueryEntry(BuildMode)
                .Replace("{Pre}", TopAmount.HasValue ? $"TOP({TopAmount})" : string.Empty)
                .Replace("{ColumnsDelimitedList}", Columns)
                .Replace("{TableName}", 
                (joinDefinitions != null && BuildMode == BuildMode.Select)
                    ? joinDefinitions.Build(out properties)
                    : BuildMode == BuildMode.Insert 
                        ? Model.TableName 
                        : $"FROM {Model.TableName}"
                ));


            if (BuildMode == BuildMode.Update)
            {
                query.Append(updateBody);
            }

            if (BuildMode != BuildMode.Insert && !string.IsNullOrWhiteSpace(whereClause))
            {
                query.Append($" WHERE {whereClause}");
            }

            if(BuildMode == BuildMode.Insert)
            {
                query.Append("@");
                query.Append(string.Join(", @", properties.Select(p => p.Name)));
                query.Append(")");
            }

            return query.ToString();
        }

        private static string ResolveColumnByJoin(IJoinDefinition joinDefinition, string name)
        {
            if (joinDefinition.Parent is IDbModel parent)
            {
                var columnName = parent.ResolveColumnName(name, true);
                if (!string.IsNullOrWhiteSpace(columnName))
                {
                    return columnName;
                }
            }

            if (joinDefinition.Child is IDbModel child)
            {
                var columnName = child.ResolveColumnName(name, true);
                if (!string.IsNullOrWhiteSpace(columnName))
                {
                    return columnName;
                }
            }

            return string.Empty;
        }

        private string ResolveColumn(string name)
        {
            foreach (var joinDefinition in joinDefinitions!)
            {
                var columnName = ResolveColumnByJoin(joinDefinition, name);
                if (!string.IsNullOrWhiteSpace(columnName))
                {
                    return columnName;
                }
            }

            return string.Empty;
        }

        private static string GetQueryEntry(BuildMode buildMode)
        {
            return buildMode switch
            {
                BuildMode.Insert => "INSERT INTO {TableName} ({ColumnsDelimitedList}) VALUES ( ",
                BuildMode.Select => "SELECT {Pre} {ColumnsDelimitedList} {TableName}",
                BuildMode.Update => "UPDATE {TableName} SET ",
                _ => throw new NotSupportedException()
            };
        }

        public DefaultQueryBuilder(TModel model, IJoinDefinitionBuilder? joinDefinitions = null)
        {
            Model = model;
            this.joinDefinitions = joinDefinitions;
        }

        public string Columns => columns ??= (joinDefinitions == null
            ? Model.FullyQualifiedColumnDelimitedList
            : GetColumns(joinDefinitions));

        public int? TopAmount { get; set; }
        public BuildMode BuildMode { get; set; }
        public TModel Model { get; }
        public string Query => GenerateQuery();

        public void GenerateUpdateBody<TRequest>(TRequest request)
        {
            updateBody = GenerateWhereClause(request, ", ");
        }

        public void GenerateWhereClause<TRequest>(TRequest request)
        {
            whereClause = GenerateWhereClause(request, " AND ");
        }

    }
}
