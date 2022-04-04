namespace DiabetesManagement.Shared.RequestHandlers.ApiToken
{
    public class Commands
    {
        public const string SaveApiToken = "Save-API-Token";
        public const string UpdateApiToken = "Update-API-Token";

        public const string InsertApiTokenCommand = @"INSERT INTO [dbo].[INVENTORY] (
                [API_TOKENID], 
                [KEY], 
                [SECRET], 
                [CREATED]
            ) VALUES (
                @apiTokenId,
                @key,
                @secret,
                @created
            ); SELECT @apiTokenId";
    }
}
