namespace DiabetesManagement.Shared.RequestHandlers.User
{
    public static class Queries
    {
        public const string GetUser = "Get-User";

        public const string GetUserQuery = "SELECT TOP (1) [USERID], [EMAILADDRESS], [USERNAME], [CREATED], [MODIFIED] FROM [dbo].[USER]  @@whereClause";

        public static string GetWhereClause(GetRequest request)
        {
            if (request.UserId.HasValue)
            {
                return "WHERE [USERID] = @userId";
            }
            var query = "WHERE 1 + 1";
            var condition = "AND";

            if (!string.IsNullOrWhiteSpace(request.EmailAddress))
            {
                query += " AND [EMAILADDRESS] = @emailAddress";
                condition = "OR";
            }

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                query += $" {condition} [USERNAME] = @userName";
            }

            return query;
        }
    }
}
