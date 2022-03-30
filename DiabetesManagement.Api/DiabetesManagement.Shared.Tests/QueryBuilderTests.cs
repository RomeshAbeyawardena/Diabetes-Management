
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
        public void UpdateQuery()
        {
            defaultQueryBuilder!.BuildMode = Enumerations.BuildMode.Update;

            var request = new GetRequest
            {
                InventoryId = Guid.NewGuid(),
            };

            var request2 = new PutRequest
            {
                Modified = DateTime.UtcNow
            };

            defaultQueryBuilder.GenerateUpdateBody(request2);
            defaultQueryBuilder.GenerateWhereClause(request);

            Assert.AreEqual("UPDATE [dbo].[INVENTORY] SET [INVENTORY].[Modified] = @Modified WHERE [INVENTORY].[InventoryId] = @InventoryId", defaultQueryBuilder.Query);

            request2 = new PutRequest
            {
                DefaultType = "Banana",
                Modified = DateTime.UtcNow
            };

            defaultQueryBuilder.GenerateUpdateBody(request2);
            defaultQueryBuilder.GenerateWhereClause(request);

            Assert.AreEqual("UPDATE [dbo].[INVENTORY] SET [INVENTORY].[DEFAULT_TYPE] = @DefaultType, [INVENTORY].[Modified] = @Modified WHERE [INVENTORY].[InventoryId] = @InventoryId", defaultQueryBuilder.Query);
        }

        [Test]
        public void InsertQuery()
        {
            defaultQueryBuilder!.BuildMode = Enumerations.BuildMode.Insert;
            Assert.AreEqual("INSERT INTO [dbo].[INVENTORY] ([INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified]) VALUES ( @InventoryId, @UserId, @Key, @DefaultType, @Hash, @Created, @Modified); SELECT @InventoryId", defaultQueryBuilder.Query);
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
