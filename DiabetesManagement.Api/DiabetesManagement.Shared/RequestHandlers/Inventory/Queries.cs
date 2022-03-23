using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    public class Queries
    {
        public const string GetInventory = "Get-Inventory";

        public const string InventoryQuery = @"SELECT TOP(1) [I].[INVENTORYID], [I].[DEFAULT_TYPE] [DefaultType], [I].[KEY], [I].[USERID],
                    [I].[CREATED], [I].[MODIFIED] FROM [dbo].[INVENTORY][I] 
                    @@whereClause";

        public static string GetInventoryWhereClause(Guid? inventoryId)
        {
            return inventoryId.HasValue && inventoryId != default
                ? "WHERE [I].[INVENTORYID] = @id"
                : "WHERE [I].[KEY] = @key AND [I].[USERID] = @userId ";
        }
    }
}
