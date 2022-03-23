using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	            [CREATED]
            ) VALUES (
                @inventoryId,
                @key,
                @userId,
                @defaultType,
                @created
            ); SELECT @inventoryId";
    }
}
