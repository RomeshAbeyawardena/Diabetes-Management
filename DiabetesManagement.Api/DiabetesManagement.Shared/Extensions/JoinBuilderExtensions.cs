using DiabetesManagement.Shared.Contracts;
using DiabetesManagement.Shared.Enumerations;

namespace DiabetesManagement.Shared.Extensions
{
    public static class JoinBuilderExtensions
    {
        public static string Build(this IJoinDefinitionBuilder joinDefinitions, out string columns)
        {
            columns = string.Empty;

            var query = "FROM ";
            foreach(var (parentType, definitions) in joinDefinitions.ParentJoinDefinitions)
            {
                foreach(var definition in definitions)
                {
                    if(definition.Parent is IDbModel parentModel && definition.Child is IDbModel childModel)
                    {
                        query += $"{parentModel.TableName} {Enum.GetName(typeof(JoinType), definition.JoinType)!.ToUpper()} JOIN {childModel.TableName} " +
                            $"ON {parentModel.ResolveColumnName(definition.ParentRelationProperty, true)} = {childModel.ResolveColumnName(definition.ChildRelationProperty, true)}";

                        columns += childModel.FullyQualifiedColumnDelimitedList;
                    }
                }
            }

            return query;
        }
    }
}
