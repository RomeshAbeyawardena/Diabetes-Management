using DiabetesManagement.Shared.Contracts;
using DiabetesManagement.Shared.Enumerations;
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

    public JoinDefinition()
        : this(null!, null!)
    {

    }

    public JoinDefinition(Expression<Func<TParent, object>> parentRelationProperty, Expression<Func<TChild, object>> childRelationProperty, JoinType joinType = JoinType.Inner)
    {
        joinDefinitionVisitor = new();
        ParentRelationProperty = parentRelationProperty;
        ChildRelationProperty = childRelationProperty;
        JoinType = joinType;
    }

    public virtual Expression<Func<TParent, object>> ParentRelationProperty { get; set; }

    public virtual Expression<Func<TChild, object>> ChildRelationProperty { get; set; }

    public TParent Parent => parent ??= Activator.CreateInstance<TParent>();

    public TChild Child => child ??= Activator.CreateInstance<TChild>();

    public JoinType JoinType { get; }

    public IJoinDefinition Definition => this;

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

    object IJoinDefinition.Parent => Parent!;
    object IJoinDefinition.Child => Child!;
}
