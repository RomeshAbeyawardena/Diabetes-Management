namespace DiabetesManagement.Features.User;

public class GetRequest
{
    public Guid? UserId { get; set; }
    public string? EmailAddress { get; set; }
    public string? Password { get; set; }
}
