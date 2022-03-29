using DiabetesManagement.Shared.Contracts;
using DiabetesManagement.Shared.Enumerations;
using DiabetesManagement.Shared.Extensions;
using System.Text;

namespace DiabetesManagement.Shared.Defaults
{
    public class DefaultQueryBuilder<TModel> : IQueryBuilder<TModel>
        where TModel : IDbModel
    {
        private readonly IJoinDefinitionBuilder? joinDefinitions;
        private string? columns;

        private string GetColumns(IEnumerable<IJoinDefinition> joinDefinitions)
        {
            var columnList = new StringBuilder();
            
            void ExtractColumns(IDbModel model)
            {
                if (columnList.Length != 0)
                {
                    columnList.Append(",");
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
            var query = new StringBuilder();
            query.Append(GetQueryEntry(BuildMode)
                .Replace("{Pre}", TopAmount.HasValue ? $"TOP({TopAmount})" : string.Empty)
                .Replace("{ColumnsDelimitedList}", Columns)
                .Replace("{TableName}", 
                (joinDefinitions != null && BuildMode == BuildMode.Select)
                    ? joinDefinitions.Build(out var columns)
                    : $"FROM {Model.TableName}"
                ));


            return query.ToString();
        }

        private string GetQueryEntry(BuildMode buildMode)
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
    }
}
