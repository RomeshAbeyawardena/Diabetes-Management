using DiabetesManagement.Shared.Contracts;
using DiabetesManagement.Shared.Enumerations;
using System.Linq.Expressions;

namespace DiabetesManagement.Shared.Defaults
{
    public class DefaultJoinDefinitionBuilder : List<IJoinDefinition>, IJoinDefinitionBuilder
    {
        private void AddOrUpdate(Type parentType, IJoinDefinition joinDefinition)
        {
            if (!ParentJoinDefinitions.TryAdd(parentType, new[] { joinDefinition }))
            {
                var items = ParentJoinDefinitions[parentType];

                ParentJoinDefinitions[parentType] = items.Append(joinDefinition);
            }

            Add(joinDefinition);
        }

        public DefaultJoinDefinitionBuilder()
        {
            ParentJoinDefinitions = new Dictionary<Type, IEnumerable<IJoinDefinition>>();
        }

        public IDictionary<Type, IEnumerable<IJoinDefinition>> ParentJoinDefinitions { get; }

        public IJoinDefinitionBuilder Add<TParent, TChild>(Expression<Func<TParent, object>> parentRelationProperty, Expression<Func<TChild, object>> childRelationProperty, JoinType joinType = JoinType.Inner)
        {
            var joinDefinition = (IJoinDefinition)Activator.CreateInstance(typeof(DefaultJoinDefinition<TParent, TChild>), parentRelationProperty, childRelationProperty, joinType)!;

            var parentType = typeof(TParent);

            AddOrUpdate(parentType, joinDefinition);
            return this;
        }

        public IJoinDefinitionBuilder Add<TParent, TChild>(Action<IJoinDefinition<TParent, TChild>> build)
        {
            var joinDefinition = Activator.CreateInstance<DefaultJoinDefinition<TParent, TChild>>();
            build(joinDefinition);

            var parentType = typeof(TParent);

            AddOrUpdate(parentType, joinDefinition);
            return this;
        }
    }
}
