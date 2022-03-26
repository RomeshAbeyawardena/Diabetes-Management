using DiabetesManagement.Shared.Models;
using NUnit.Framework;
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
            var inventory = new Inventory();
            Assert.AreEqual("InventoryId,UserId,Key,DefaultType,Hash,Created,Modified", inventory.ColumnDelimitedList);
        }
    }
}
