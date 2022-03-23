using System;

namespace DiabetesManagement.Api.RequestHandlers
{
    public static class Queries
    {
        public const string InventoryQuery = @"SELECT TOP(1) [I].[INVENTORYID], [I].[DEFAULT_TYPE] [DefaultType], [I].[KEY], [I].[USERID],
                    [I].[CREATED], [I].[MODIFIED] FROM [dbo].[INVENTORY][I] 
                    @@whereClause";

        public const string InventoryHistoryQuery = @"SELECT TOP(1) [I].[INVENTORYID], [I].[KEY], [I].[USERID],
                    [I].[CREATED], [I].[MODIFIED], [I].[DEFAULT_TYPE] [DefaultType],
                    [IH].[INVENTORY_HISTORYID] [InventoryHistoryId], [IH].[VERSION],
                    [IH].[ITEMS], [IH].[TYPE], [IH].[CREATED] [InventoryHistoryCreated]
                FROM [dbo].[INVENTORY_HISTORY] [IH]
                INNER JOIN [dbo].[INVENTORY][I]
                ON [IH].[INVENTORYID] = [I].[INVENTORYID]
                    @@whereClause
                ORDER BY [VERSION] DESC";

        public static string GetInventoryWhereClause(Guid? inventoryId)
        {
            return inventoryId.HasValue && inventoryId != default
                ? "WHERE [I].[INVENTORYID] = @id"
                : "WHERE [I].[KEY] = @key AND [I].[USERID] = @userId ";
        }

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
