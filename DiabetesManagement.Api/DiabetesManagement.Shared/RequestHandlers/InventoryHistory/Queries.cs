using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Shared.RequestHandlers.InventoryHistory
{
    public static class Queries
    {
        public const string GetInventoryHistory = "Get-Inventory-History";

        public const string InventoryHistoryQuery = @"SELECT TOP(1) [I].[INVENTORYID], [I].[KEY], [I].[USERID],
                    [I].[CREATED], [I].[MODIFIED], [I].[DEFAULT_TYPE] [DefaultType],
                    [IH].[INVENTORY_HISTORYID] [InventoryHistoryId], [IH].[VERSION],
                    [IH].[ITEMS], [IH].[TYPE], [IH].[CREATED] [InventoryHistoryCreated]
                FROM [dbo].[INVENTORY_HISTORY] [IH]
                INNER JOIN [dbo].[INVENTORY][I]
                ON [IH].[INVENTORYID] = [I].[INVENTORYID]
                    @@whereClause
                ORDER BY [VERSION] DESC";

        public static string GetInventoryHistoryWhereClause(int? version)
        {
            var whereClause = "WHERE [I].[KEY] = @key AND [I].[USERID] = @userId AND [IH].[TYPE] = @type";

            if (version.HasValue)
            {
                whereClause += " AND [IH].[Version] = @version";
            }

            return whereClause;
        }
    }
}
