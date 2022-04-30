namespace Ledger.Features;

public static partial class Permissions
{
    public static IEnumerable<string> LedgerAdminPermissions => new [] { Account_Edit, Account_View, Ledger_Edit, Ledger_View };
}
