using DiabetesManagement.Shared.Enumerations;
using System.Linq.Expressions;
using System.Reflection;

namespace DiabetesManagement.Shared.Contracts
{
    public interface IJoinDefinition
    {
        JoinType JoinType { get; }
        PropertyInfo ParentRelationProperty { get; }
        PropertyInfo ChildRelationProperty { get; }
        object Parent { get; }
        object Child { get; }
    }

    public interface IJoinDefinition<TParent, TChild> : IJoinDefinition
    {
        IJoinDefinition Definition { get; }
        new Expression<Func<TParent, object>> ParentRelationProperty { get; set; }
        new Expression<Func<TChild, object>> ChildRelationProperty { get; set; }
        new TParent Parent { get; }
        new TChild Child { get; }
    }
}
