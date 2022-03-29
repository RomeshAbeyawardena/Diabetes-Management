
using DiabetesManagement.Shared.Defaults;
using DiabetesManagement.Shared.Extensions;
using DiabetesManagement.Shared.RequestHandlers.Inventory;
using NUnit.Framework;
using System;

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
        public void SelectQuery()
        {
            defaultQueryBuilder!.BuildMode = Enumerations.BuildMode.Select;
            Assert.AreEqual("SELECT  [INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified] FROM [dbo].[INVENTORY]", defaultQueryBuilder!.Query);

            defaultQueryBuilder = new(inventory!, inventory!
                .JoinDefinitionsBuilder(builder => builder
                .Add<Models.Inventory, Models.InventoryHistory>(i => i.InventoryId, iH => iH.InventoryId)));
            defaultQueryBuilder.BuildMode = Enumerations.BuildMode.Select;

            Assert.AreEqual("SELECT  [INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified],[INVENTORY_HISTORY].[INVENTORY_HISTORYID], [INVENTORY_HISTORY].[InventoryId], [INVENTORY_HISTORY].[Version], [INVENTORY_HISTORY].[Type], [INVENTORY_HISTORY].[Items], [INVENTORY_HISTORY].[Hash], [INVENTORY_HISTORY].[Created] FROM [dbo].[INVENTORY] INNER JOIN [dbo].[INVENTORY_HISTORY] ON [INVENTORY].[InventoryId] = [INVENTORY_HISTORY].[InventoryId]", defaultQueryBuilder!.Query);

            defaultQueryBuilder.TopAmount = 10;

            Assert.AreEqual("SELECT TOP(10) [INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified],[INVENTORY_HISTORY].[INVENTORY_HISTORYID], [INVENTORY_HISTORY].[InventoryId], [INVENTORY_HISTORY].[Version], [INVENTORY_HISTORY].[Type], [INVENTORY_HISTORY].[Items], [INVENTORY_HISTORY].[Hash], [INVENTORY_HISTORY].[Created] FROM [dbo].[INVENTORY] INNER JOIN [dbo].[INVENTORY_HISTORY] ON [INVENTORY].[InventoryId] = [INVENTORY_HISTORY].[InventoryId]", defaultQueryBuilder!.Query);

            var request = new GetRequest
            {
                InventoryId = Guid.NewGuid(),
            };

            defaultQueryBuilder.GenerateWhereClause(request);

            Assert.AreEqual("SELECT TOP(10) [INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified],[INVENTORY_HISTORY].[INVENTORY_HISTORYID], [INVENTORY_HISTORY].[InventoryId], [INVENTORY_HISTORY].[Version], [INVENTORY_HISTORY].[Type], [INVENTORY_HISTORY].[Items], [INVENTORY_HISTORY].[Hash], [INVENTORY_HISTORY].[Created] FROM [dbo].[INVENTORY] INNER JOIN [dbo].[INVENTORY_HISTORY] ON [INVENTORY].[InventoryId] = [INVENTORY_HISTORY].[InventoryId] WHERE [INVENTORY].[InventoryId] = @InventoryId", defaultQueryBuilder!.Query);

            request = new GetRequest
            {
                InventoryId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Key = ""
            };

            defaultQueryBuilder.GenerateWhereClause(request);

            Assert.AreEqual("SELECT TOP(10) [INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified],[INVENTORY_HISTORY].[INVENTORY_HISTORYID], [INVENTORY_HISTORY].[InventoryId], [INVENTORY_HISTORY].[Version], [INVENTORY_HISTORY].[Type], [INVENTORY_HISTORY].[Items], [INVENTORY_HISTORY].[Hash], [INVENTORY_HISTORY].[Created] FROM [dbo].[INVENTORY] INNER JOIN [dbo].[INVENTORY_HISTORY] ON [INVENTORY].[InventoryId] = [INVENTORY_HISTORY].[InventoryId] WHERE [INVENTORY].[InventoryId] = @InventoryId AND [INVENTORY].[Key] = @Key AND [INVENTORY].[UserId] = @UserId", defaultQueryBuilder!.Query);

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
