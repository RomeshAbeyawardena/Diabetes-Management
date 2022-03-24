namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    public static class Commands
    {
        public const string SaveInventory = "Save-Inventory";
        public const string UpdateInventory = "Update-Inventory";

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
                [Hash],
	            [CREATED]
            ) VALUES (
                @inventoryId,
                @key,
                @userId,
                @defaultType,
                @hash,
                @created
            ); SELECT @inventoryId";
    }
}
