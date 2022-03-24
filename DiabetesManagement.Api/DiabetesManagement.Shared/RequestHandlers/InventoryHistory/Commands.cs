namespace DiabetesManagement.Shared.RequestHandlers.InventoryHistory
{
    public static class Commands
    {
        public const string SaveInventoryHistory = "Save-Inventory-History";

        public const string InsertInventoryHistoryCommand = @"INSERT INTO [dbo].[INVENTORY_HISTORY] (
                                [INVENTORY_HISTORYID],
                                [INVENTORYID],
                                [VERSION],
                                [TYPE],
                                [ITEMS],
                                [HASH],
                                [CREATED]
                            ) VALUES (
                                @inventoryHistoryId,
                                @inventoryId,
                                @version,
                                @type,
                                @items,
                                @hash,
                                @created
                            ); SELECT @inventoryHistoryId";
    }
}
