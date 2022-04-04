using DiabetesManagement.Shared.Defaults;
using DiabetesManagement.Shared.Extensions;
using DiabetesManagement.Shared.Models;
using NUnit.Framework;

namespace DiabetesManagement.Shared.Tests
{
    public class JoinDefinitionTests
    {
        private DefaultJoinDefinition<Inventory, InventoryHistory>? joinDefinition;

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
            Assert.AreEqual("[INVENTORY_HISTORY].[InventoryId]", joinDefinition.Child.ResolveColumnName(definition.ChildRelationProperty, true));

            Assert.AreEqual("[InventoryId]", joinDefinition.Parent.ResolveColumnName(definition.ParentRelationProperty, false));
            Assert.AreEqual("[InventoryId]", joinDefinition.Child.ResolveColumnName(definition.ChildRelationProperty, false));

            var definitionBuilder = new DefaultJoinDefinitionBuilder();
            definitionBuilder.Add<Inventory, InventoryHistory>(b => { b.ParentRelationProperty = i => i.InventoryId; b.ChildRelationProperty = iH => iH.InventoryId; });
            Assert.AreEqual("FROM [dbo].[INVENTORY] INNER JOIN [dbo].[INVENTORY_HISTORY] ON [INVENTORY].[InventoryId] = [INVENTORY_HISTORY].[InventoryId]", definitionBuilder.Build(out var columns));
            Assert.AreEqual("[INVENTORY_HISTORY].[INVENTORY_HISTORYID], [INVENTORY_HISTORY].[InventoryId], [INVENTORY_HISTORY].[Version], [INVENTORY_HISTORY].[Type], [INVENTORY_HISTORY].[Items], [INVENTORY_HISTORY].[Hash], [INVENTORY_HISTORY].[Created]", columns);

            definitionBuilder = new DefaultJoinDefinitionBuilder();
            definitionBuilder.Add<Inventory, InventoryHistory>(i => i.InventoryId, iH => iH.InventoryId);
            Assert.AreEqual("FROM [dbo].[INVENTORY] INNER JOIN [dbo].[INVENTORY_HISTORY] ON [INVENTORY].[InventoryId] = [INVENTORY_HISTORY].[InventoryId]", definitionBuilder.Build(out columns));
            Assert.AreEqual("[INVENTORY_HISTORY].[INVENTORY_HISTORYID], [INVENTORY_HISTORY].[InventoryId], [INVENTORY_HISTORY].[Version], [INVENTORY_HISTORY].[Type], [INVENTORY_HISTORY].[Items], [INVENTORY_HISTORY].[Hash], [INVENTORY_HISTORY].[Created]", columns);
        }
    }
}
