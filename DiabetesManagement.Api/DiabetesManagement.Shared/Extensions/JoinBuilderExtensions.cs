using DiabetesManagement.Shared.Contracts;
using DiabetesManagement.Shared.Enumerations;
using System.Reflection;

namespace DiabetesManagement.Shared.Extensions
{
    public static class JoinBuilderExtensions
    {
        public static string Build(this IJoinDefinitionBuilder joinDefinitions, out IEnumerable<PropertyInfo> properties)
        {
            var props = new List<PropertyInfo>();
            properties = props;
            var query = "FROM ";
            foreach(var (parentType, definitions) in joinDefinitions.ParentJoinDefinitions)
            {
                foreach(var definition in definitions)
                {
                    if(definition.Parent is IDbModel parentModel && definition.Child is IDbModel childModel)
                    {
                        query += $"{parentModel.TableName} {Enum.GetName(typeof(JoinType), definition.JoinType)!.ToUpper()} JOIN {childModel.TableName} " +
                            $"ON {parentModel.ResolveColumnName(definition.ParentRelationProperty, true)} = {childModel.ResolveColumnName(definition.ChildRelationProperty, true)}";

                        props.AddRange(parentModel.Properties);
                        props.AddRange(childModel.Properties);
                    }
                }
            }

            return query;
        }
    }
}
