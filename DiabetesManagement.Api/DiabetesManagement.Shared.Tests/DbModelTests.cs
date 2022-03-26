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
            Assert.AreEqual("[INVENTORY].[InventoryId], [INVENTORY].[UserId], [INVENTORY].[Key], [INVENTORY].[DefaultType], [INVENTORY].[Hash], [INVENTORY].[Created], [INVENTORY].[Modified]", inventory.FullyQualifiedColumnDelimitedList);
            Assert.AreEqual("SELECT [InventoryId],[UserId],[Key],[DefaultType],[Hash],[Created],[Modified] FROM [dbo].[INVENTORY] ", inventory.Build());
            Assert.AreEqual("SELECT TOP(20) [InventoryId],[UserId],[Key],[DefaultType],[Hash],[Created],[Modified] FROM [dbo].[INVENTORY] ", inventory.Build(20));
        }
    }
}
