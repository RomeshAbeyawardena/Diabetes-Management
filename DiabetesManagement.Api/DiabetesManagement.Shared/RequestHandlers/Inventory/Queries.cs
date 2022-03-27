namespace DiabetesManagement.Shared.RequestHandlers.Inventory
{
    public class Queries
    {
        public const string GetInventory = "Get-Inventory";

        //public static string InventoryQuery = $"SELECT TOP(1) { inventory.ColumnDelimitedList } FROM { inventory.TableName } @@whereClause";

        //public static string GetInventoryWhereClause(Guid? inventoryId)
        //{
        //    return inventoryId.HasValue && inventoryId != default
        //        ? $"{inventory.WhereClause}"
        //        : "WHERE [I].[DEFAULT_TYPE] = @type AND [I].[KEY] = @key AND [I].[USERID] = @userId ";
        //}
    }
}
