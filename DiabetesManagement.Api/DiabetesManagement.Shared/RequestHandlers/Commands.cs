namespace DiabetesManagement.Shared.RequestHandlers
{
    public static class Commands
    {
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
