using NUnit.Framework;
using DiabetesManagement.Shared.RequestHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiabetesManagement.Shared.Extensions;

namespace DiabetesManagement.Shared.Tests
{
    public class DbModelTests
    {
        [Test]
        public void Test()
        {
            var inventory = new Models.Inventory();
            Assert.AreEqual("[InventoryId],[UserId],[Key],[DefaultType],[Hash],[Created],[Modified]", inventory.ColumnDelimitedList);
            Assert.AreEqual("[INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DefaultType], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified]", 
                inventory.FullyQualifiedColumnDelimitedList);
            Assert.AreEqual("SELECT [INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DefaultType], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified] FROM [dbo].[INVENTORY] ", 
                inventory.Build());
            Assert.AreEqual("SELECT TOP(20) [INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DefaultType], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified] FROM [dbo].[INVENTORY] ",
                inventory.Build(20));

            Assert.AreEqual("SELECT TOP(20) [INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DefaultType], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified], " +
                "[InventoryHistory].[InventoryHistoryId], [InventoryHistory].[InventoryId], [InventoryHistory].[Version], [InventoryHistory].[Type], [InventoryHistory].[Items], [InventoryHistory].[Hash], [InventoryHistory].[Created] " +
                "FROM [dbo].[INVENTORY] " +
                "INNER JOIN [dbo].[InventoryHistory] ON [INVENTORY].[InventoryId] = [InventoryHistory].[InventoryId]", 
                inventory.Build(20, "", b => b.Add<Models.Inventory, Models.InventoryHistory>(p => p.InventoryId, c => c.InventoryId)));
        }
    }
}
