namespace DiabetesManagement.Models;

public class SessionValidation
{
    public Guid? SessionId { get; set; }
    public Guid? UserId { get; set; }
    public bool IsValid { get; set; }
}
