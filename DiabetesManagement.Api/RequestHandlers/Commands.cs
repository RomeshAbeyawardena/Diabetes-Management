namespace DiabetesManagement.Api.RequestHandlers
{
    public static class Commands
    {
        public const string UpdateInventoryCommand = @"
                UPDATE [dbo].[INVENTORY] 
                SET 
                    [KEY] = @key,
                    [DEFAULT_TYPE] = @defaultType,
                    [MODIFIED] = @modified   
                WHERE [INVENTORYID] = @inventoryid; SELECT @inventoryid";

        public const string InsertInventoryCommand = @"INSERT INTO [dbo].[Inventory] (
                [INVENTORYID],
	            [KEY],
	            [USERID],
                [DEFAULT_TYPE],
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
}
