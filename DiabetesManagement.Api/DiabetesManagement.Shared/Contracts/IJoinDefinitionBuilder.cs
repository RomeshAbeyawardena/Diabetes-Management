using DiabetesManagement.Shared.Enumerations;
using System.Linq.Expressions;

namespace DiabetesManagement.Shared.Contracts
{
    public interface IJoinDefinitionBuilder : IEnumerable<IJoinDefinition>
    {
        IDictionary<Type, IEnumerable<IJoinDefinition>> ParentJoinDefinitions { get; }
        IJoinDefinitionBuilder Add<TParent, TChild>(Action<IJoinDefinition<TParent, TChild>> build);
        IJoinDefinitionBuilder Add<TParent, TChild>(Expression<Func<TParent, object>> parentRelationProperty, Expression<Func<TChild, object>> childRelationProperty, JoinType joinType = JoinType.Inner);
    }
}
