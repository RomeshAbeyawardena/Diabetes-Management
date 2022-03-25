namespace DiabetesManagement.Shared.RequestHandlers.ApiTokenClaim
{
    public static class Queries
    {
        public const string GetApiTokenClaims = "Get-Api-Token-Claims";

        public const string GetApiTokenClaimsQuery = @"
            SELECT [API_TOKENID] [API_TOKEN_CLAIMID], [KEY], [SECRET], [CLAIM], [CREATED,
                [CREATED] [ApiTokenCreated]
            FROM [API_TOKEN] [AT]
            INNER JOIN [API_TOKEN_CLAIM] [ATC]
            ON [ATC].[API_TOKENID] = [AT].[API_TOKENID]
        @@whereClause";

        public static string GetApiTokenClaimsWhereClause(GetRequest request)
        {
            if (request.ApiTokenId.HasValue)
            {
                return "[AT].[API_TOKENID] = @apiTokenId";
            }

            var query = " 1 + 1";

            if (!string.IsNullOrEmpty(request.Claim))
            {
                query += " AND [ATC].[CLAIM] = @claim";
            }

            return query;
        }
    }
}
