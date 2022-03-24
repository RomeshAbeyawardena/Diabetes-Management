namespace DiabetesManagement.Shared.RequestHandlers.User
{
    public static class Commands
    {
        public const string SaveUser = "Save-User";
        public const string InsertUser = @"INSERT INTO [dbo].[User] (
            [USERID], 
            [EMAILADDRESS], 
            [USERNAME], 
            [HASH]
            [CREATED]
        ) VALUES (
            @userId,
            @emailAddress,
            @userName,
            @hash,
            @created
        ); SELECT @userId";
    }
}
