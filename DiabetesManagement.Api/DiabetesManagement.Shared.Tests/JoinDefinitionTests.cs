using DiabetesManagement.Shared.Extensions;
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

            Assert.AreEqual("[InventoryId]", joinDefinition.Parent.ResolveColumnName(definition.ParentRelationProperty, false));
            Assert.AreEqual("[InventoryId]", joinDefinition.Child.ResolveColumnName(definition.ChildRelationProperty, false));

            var definitionBuilder = new JoinDefinitionBuilder();
            definitionBuilder.Add<Inventory, InventoryHistory>(b => { b.ParentRelationProperty = i => i.InventoryId; b.ChildRelationProperty = iH => iH.InventoryId; });
            Assert.AreEqual("FROM [dbo].[INVENTORY] INNER JOIN [dbo].[InventoryHistory] ON [INVENTORY].[InventoryId] = [InventoryHistory].[InventoryId]", definitionBuilder.Build());


            definitionBuilder = new JoinDefinitionBuilder();
            definitionBuilder.Add<Inventory, InventoryHistory>(i => i.InventoryId, iH => iH.InventoryId);
            Assert.AreEqual("FROM [dbo].[INVENTORY] INNER JOIN [dbo].[InventoryHistory] ON [INVENTORY].[InventoryId] = [InventoryHistory].[InventoryId]", definitionBuilder.Build());
        }
    }
}
