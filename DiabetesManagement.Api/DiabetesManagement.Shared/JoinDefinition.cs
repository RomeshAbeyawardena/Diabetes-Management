using DiabetesManagement.Shared.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace DiabetesManagement.Shared;
public class JoinDefinition<TParent, TChild> : IJoinDefinition<TParent, TChild>
{
    private TParent? parent;
    private TChild? child;
    private PropertyInfo? parentProperty;
    private PropertyInfo? childProperty;
    private readonly JoinDefinitionVisitor joinDefinitionVisitor;
    internal class JoinDefinitionVisitor : ExpressionVisitor
    {
        public PropertyInfo? Property { get; private set; }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member is PropertyInfo property)
            {
                Property = property;
            }
            return base.VisitMember(node);
        }
    }

    public JoinDefinition(Expression<Func<TParent, object>> parentRelationProperty, Expression<Func<TChild, object>> childRelationProperty)
    {
        joinDefinitionVisitor = new();
        ParentRelationProperty = parentRelationProperty;
        ChildRelationProperty = childRelationProperty;
    }

    public Expression<Func<TParent, object>> ParentRelationProperty { get; }

    public Expression<Func<TChild, object>> ChildRelationProperty { get; }

    public TParent Parent => parent ??= Activator.CreateInstance<TParent>();

    public TChild Child => child ??= Activator.CreateInstance<TChild>();

    PropertyInfo IJoinDefinition.ParentRelationProperty 
    {
        get {
            if (parentProperty == null)
                joinDefinitionVisitor.Visit(ParentRelationProperty); 
                    
            return parentProperty ??= joinDefinitionVisitor.Property!;
        }
    }

    PropertyInfo IJoinDefinition.ChildRelationProperty
    {
        get
        {
            if (childProperty == null)
                joinDefinitionVisitor.Visit(ChildRelationProperty);

            return childProperty ??= joinDefinitionVisitor.Property!;
        }
    }

    public IJoinDefinition Definition => this;
}
