namespace DiabetesManagement.Shared.RequestHandlers.ApiToken
{
    public class Queries
    {
        public const string GetApiToken = "Get-API-Token";

        public const string GetApiTokenQuery = @"SELECT [API_TOKENID], [KEY], [SECRET], [CREATED] @@whereClause";

        public static string GetApiTokenWhereClause(GetRequest request)
        {
            if (request.ApiTokenId.HasValue)
            {
                return " WHERE [API_TOKENID] = @apiTokenId";
            }

            var query = " 1 = 1";

            if (!string.IsNullOrEmpty(request.Key))
            {
                query += " [KEY] = @key";
            }

            if (!string.IsNullOrEmpty(request.Secret))
            {
                query += " AND [SECRET] = @secret";
            }

            return query;
        }
    }
}
