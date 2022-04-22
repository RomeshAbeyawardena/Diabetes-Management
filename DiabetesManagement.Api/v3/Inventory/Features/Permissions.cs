namespace DiabetesManagement.Features;

public static partial class Permissions
{
    public static IEnumerable<string> SysAdmin => new[] {
        AccessToken_Edit, AccessToken_View, Application_Edit, Application_View,
        InventoryHistory_Edit, InventoryHistory_View,
        Inventory_Edit, Inventory_View,
        Session_Edit, Session_View,
        User_Edit, User_View
    };
}
