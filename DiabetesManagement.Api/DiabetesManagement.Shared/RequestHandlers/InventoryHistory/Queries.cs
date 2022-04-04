namespace DiabetesManagement.Shared.RequestHandlers.InventoryHistory
{
    public static class Queries
    {
        public const string GetInventoryHistory = "Get-Inventory-History";

        //public const string InventoryHistoryQuery = @"SELECT TOP(1) [I].[INVENTORYID], [I].[KEY], [I].[USERID],
        //            [I].[HASH], [I].[CREATED], [I].[MODIFIED], [I].[DEFAULT_TYPE] [DefaultType],
        //            [IH].[INVENTORY_HISTORYID] [InventoryHistoryId], [IH].[VERSION],
        //            [IH].[ITEMS], [IH].[TYPE], [IH].[HASH] [InventoryHistoryHash], 
        //            [IH].[CREATED] [InventoryHistoryCreated]
        //        FROM [dbo].[INVENTORY_HISTORY] [IH]
        //        INNER JOIN [dbo].[INVENTORY][I]
        //        ON [IH].[INVENTORYID] = [I].[INVENTORYID]
        //            @@whereClause
        //        ORDER BY [VERSION] DESC";

        //public static string GetInventoryHistoryWhereClause(GetRequest request)
        //{
        //    var whereClause = request.InventoryHistoryId.HasValue 
        //        ? "WHERE [IH].[INVENTORY_HISTORYID] = @inventoryHistoryId"
        //        : "WHERE [I].[KEY] = @key AND [I].[USERID] = @userId AND [IH].[TYPE] = @type";

        //    if (request.Version.HasValue)
        //    {
        //        whereClause += " AND [IH].[Version] = @version";
        //    }

        //    return whereClause;
        //}
    }
}
