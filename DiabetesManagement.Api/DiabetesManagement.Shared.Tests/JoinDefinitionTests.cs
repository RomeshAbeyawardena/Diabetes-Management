using DiabetesManagement.Shared.Models;
using NUnit.Framework;

namespace DiabetesManagement.Shared.Tests
{
    public class JoinDefinitionTests
    {
        private JoinDefinition<Inventory, InventoryHistory>? joinDefinition;

        [SetUp]
        public void SetUp()
        {
            joinDefinition = new(i => i.InventoryId, iH => iH.InventoryId);
        }

        [Test]
        public void Test()
        {
            var definition = joinDefinition!.Definition;
            Assert.IsNotNull(definition.ParentRelationProperty);
            Assert.IsNotNull(definition.ChildRelationProperty);

            Assert.AreEqual("[INVENTORY].[InventoryId]", joinDefinition.Parent.ResolveColumnName(definition.ParentRelationProperty, true));
            Assert.AreEqual("[InventoryHistory].[InventoryId]", joinDefinition.Child.ResolveColumnName(definition.ChildRelationProperty, true));
        }
    }
}
