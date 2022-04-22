using MediatR;

namespace DiabetesManagement.Features.User;

public class GetRequest : IRequest<Models.User>
{
    public Guid? UserId { get; set; }
    public string? EmailAddress { get; set; }
    public string? Password { get; set; }
    public bool AuthenticateUser { get; set; }
}
