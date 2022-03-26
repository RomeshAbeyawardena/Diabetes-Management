using System.Linq.Expressions;
using System.Reflection;

namespace DiabetesManagement.Shared.Contracts
{
    public interface IJoinDefinition
    {
        PropertyInfo ParentRelationProperty { get; }
        PropertyInfo ChildRelationProperty { get; }
    }

    public interface IJoinDefinition<TParent, TChild> : IJoinDefinition
    {
        new Expression<Func<TParent, object>> ParentRelationProperty { get; }
        new Expression<Func<TChild, object>> ChildRelationProperty { get; }
        TParent Parent { get; }
        TChild Child { get; }
    }
}
