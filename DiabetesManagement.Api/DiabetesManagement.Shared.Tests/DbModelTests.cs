using NUnit.Framework;
using DiabetesManagement.Shared.Extensions;
using DiabetesManagement.Shared.RequestHandlers.Inventory;
using System;

namespace DiabetesManagement.Shared.Tests
{
    public class DbModelTests
    {
        [Test]
        public void BuildForSave()
        {
            var inventory = new Models.Inventory();
            Assert.AreEqual("UPDATE [dbo].[INVENTORY] SET [INVENTORY].[Modified] = @Modified", inventory.Build(Enumerations.BuildMode.Update, new Models.Inventory { Modified = DateTimeOffset.UtcNow }));
            Assert.AreEqual("UPDATE [dbo].[INVENTORY] SET [INVENTORY].[DEFAULT_TYPE] = @DefaultType, [INVENTORY].[Modified] = @Modified", inventory.Build(Enumerations.BuildMode.Update, 
                new Models.Inventory { DefaultType = "Orange", Modified = DateTimeOffset.UtcNow }));
        }

        [Test]
        public void BuildForInsert()
        {
            var inventory = new Models.Inventory();
            Assert.AreEqual("INSERT INTO [dbo].[INVENTORY] ([INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified]) " +
                "VALUES (@InventoryId, @UserId, @Key, @DEFAULT_TYPE, @Hash, @Created, @Modified); SELECT @InventoryId", inventory.Build(Enumerations.BuildMode.Insert));
        }

        [Test]
        public void Build()
        {
            var inventory = new Models.Inventory();
            Assert.AreEqual("[InventoryId],[UserId],[Key],[DEFAULT_TYPE],[Hash],[Created],[Modified]", inventory.ColumnDelimitedList);
            Assert.AreEqual("[INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified]", 
                inventory.FullyQualifiedColumnDelimitedList);
            Assert.AreEqual("SELECT [INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified] FROM [dbo].[INVENTORY] ", 
                inventory.Build());
            Assert.AreEqual("SELECT TOP(20) [INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified] FROM [dbo].[INVENTORY] ",
                inventory.Build(20));

            Assert.AreEqual("SELECT TOP(20) [INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DEFAULT_TYPE], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified], " +
                "[INVENTORY_HISTORY].[INVENTORY_HISTORYID], [INVENTORY_HISTORY].[InventoryId], [INVENTORY_HISTORY].[Version], [INVENTORY_HISTORY].[Type], [INVENTORY_HISTORY].[Items], [INVENTORY_HISTORY].[Hash], [INVENTORY_HISTORY].[Created] " +
                "FROM [dbo].[INVENTORY] " +
                "INNER JOIN [dbo].[INVENTORY_HISTORY] ON [INVENTORY].[InventoryId] = [INVENTORY_HISTORY].[InventoryId]", 
                inventory.Build(20, "", b => b.Add<Models.Inventory, Models.InventoryHistory>(p => p.InventoryId, c => c.InventoryId)));

            
        }

        [Test]
        public void GenerateWhereClause()
        {
            var inventory = new Models.Inventory();
            Assert.AreEqual(string.Empty, inventory.GenerateWhereClause(new GetRequest { }));
            Assert.AreEqual("[INVENTORY].[InventoryId] = @InventoryId", 
                inventory.GenerateWhereClause(new GetRequest { InventoryId = Guid.NewGuid() }));
            Assert.AreEqual("[INVENTORY].[InventoryId] = @InventoryId AND [INVENTORY].[Key] = @Key", 
                inventory.GenerateWhereClause(new GetRequest { InventoryId = Guid.NewGuid(), Key = "A key" }));
            Assert.AreEqual("[INVENTORY].[InventoryId] = @InventoryId AND [INVENTORY].[DEFAULT_TYPE] = @Type AND [INVENTORY].[Key] = @Key", 
                inventory.GenerateWhereClause(new GetRequest { InventoryId = Guid.NewGuid(), Key = "A key", Type = "Banana" }));
            Assert.AreEqual("[INVENTORY].[InventoryId] = @InventoryId AND [INVENTORY].[DEFAULT_TYPE] = @Type AND [INVENTORY].[Key] = @Key AND [INVENTORY].[UserId] = @UserId",
                inventory.GenerateWhereClause(new GetRequest { InventoryId = Guid.NewGuid(), Key = "A key", Type = "Banana", UserId = Guid.NewGuid() }));
        }
    }
}
