using NUnit.Framework;
using DiabetesManagement.Shared.RequestHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Shared.Tests
{
    public class DbModelTests
    {
        [Test]
        public void Test()
        {
            var inventory = new Models.Inventory();
            Assert.AreEqual("InventoryId,UserId,Key,DefaultType,Hash,Created,Modified", inventory.ColumnDelimitedList);
            Assert.AreEqual("SELECT TOP(1) InventoryId,UserId,Key,DefaultType,Hash,Created,Modified FROM [dbo].[INVENTORY] @@whereClause", RequestHandlers.Inventory.Queries.InventoryQuery);
            
        }
    }
}
