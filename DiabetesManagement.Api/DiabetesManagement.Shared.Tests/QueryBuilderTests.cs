
using DiabetesManagement.Shared.Defaults;
using DiabetesManagement.Shared.Extensions;
using NUnit.Framework;

namespace DiabetesManagement.Shared.Tests
{
    public class QueryBuilderTests
    {
        private DefaultQueryBuilder<Models.Inventory>? defaultQueryBuilder;
        private Models.Inventory? inventory;
        [SetUp]
        public void SetUp()
        {
            inventory = new();
            defaultQueryBuilder = new(inventory);
        }

        [Test]
        public void Query()
        {
            defaultQueryBuilder.BuildMode = Enumerations.BuildMode.Select;
            Assert.AreEqual("SELECT  [INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified] FROM [dbo].[INVENTORY]", defaultQueryBuilder!.Query);

            defaultQueryBuilder = new(inventory!, inventory!
                .JoinDefinitionsBuilder(builder => builder
                .Add<Models.Inventory, Models.InventoryHistory>(i => i.InventoryId, iH => iH.InventoryId)));
            defaultQueryBuilder.BuildMode = Enumerations.BuildMode.Select;

            Assert.AreEqual("SELECT  [INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified],[INVENTORY_HISTORY].[INVENTORY_HISTORYID], [INVENTORY_HISTORY].[InventoryId], [INVENTORY_HISTORY].[Version], [INVENTORY_HISTORY].[Type], [INVENTORY_HISTORY].[Items], [INVENTORY_HISTORY].[Hash], [INVENTORY_HISTORY].[Created] FROM [dbo].[INVENTORY] INNER JOIN [dbo].[INVENTORY_HISTORY] ON [INVENTORY].[InventoryId] = [INVENTORY_HISTORY].[InventoryId]", defaultQueryBuilder!.Query);
        }

        [Test]
        public void Columns()
        {
            Assert.AreEqual("[INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified]", defaultQueryBuilder!.Columns);

            defaultQueryBuilder = new(inventory!, inventory!
                .JoinDefinitionsBuilder(builder => builder
                .Add<Models.Inventory, Models.InventoryHistory>(i => i.InventoryId, iH => iH.InventoryId)));

            Assert.AreEqual("[INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified],[INVENTORY_HISTORY].[INVENTORY_HISTORYID], [INVENTORY_HISTORY].[InventoryId], [INVENTORY_HISTORY].[Version], [INVENTORY_HISTORY].[Type], [INVENTORY_HISTORY].[Items], [INVENTORY_HISTORY].[Hash], [INVENTORY_HISTORY].[Created]", defaultQueryBuilder.Columns);
        }
    }
}
