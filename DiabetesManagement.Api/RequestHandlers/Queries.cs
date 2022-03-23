using System;

namespace DiabetesManagement.Api.RequestHandlers
{
    public static class Commands
    {
        public const string UpdateInventoryCommand = @"
                UPDATE [dbo].[INVENTORY] 
                SET 
                    [KEY] = @key,
                    [DEFAULT_TYPE] = @defaultType
                    [MODIFIED] = @modified   
                WHERE [INVENTORYID] = @inventoryid; SELECT @inventoryid";

        public const string InsertInventoryCommand = @"INSERT INTO [dbo].[Inventory] (
                [INVENTORYID],
	            [KEY],
	            [USERID],
                [DEFAULT_TYPE]
	            [CREATED]
            ) VALUES (
                @inventoryId,
                @key,
                @userId,
                @defaultType,
                @created
            ); SELECT @inventoryId";

        public const string InsertInventoryHistoryCommand = @"INSERT INTO [dbo].[INVENTORY_HISTORY] (
                                [INVENTORY_HISTORYID],
                                [INVENTORYID],
                                [VERSION],
                                [TYPE],
                                [ITEMS],
                                [CREATED]
                            ) VALUES (
                                @inventoryHistoryId,
                                @inventoryId,
                                @version,
                                @type,
                                @items,
                                @created
                            ); SELECT @inventoryHistoryId";
    }
    public static class Queries
    {
        public const string InventoryQuery = @"SELECT TOP(1) [I].[INVENTORYID], [I].[DEFAULT_TYPE] [DefaultType], [I].[KEY], [I].[USERID],
                    [I].[CREATED], [I].[MODIFIED] FROM [dbo].[INVENTORY][I] 
                    @@whereClause";

        public const string InventoryHistoryQuery = @"SELECT TOP(1) [I].[INVENTORYID], [I].[KEY], [I].[USERID],
                    [I].[CREATED], [I].[MODIFIED], [I].[DEFAULT_TYPE] [DefaultType],
                    [IH].[INVENTORY_HISTORYID], [IH].[VERSION],
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
            var whereClause = "WHERE [I].[KEY] = @key AND [I].[USERID] = @userId";

            if (version.HasValue)
            {
                whereClause += " AND [IH].[Version] = @version";
            }

            return whereClause;
        }
    }
}
