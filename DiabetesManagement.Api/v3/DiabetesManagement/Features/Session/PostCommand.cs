using MediatR;

namespace DiabetesManagement.Features.Session;

public class PostCommand : IRequest<Models.Session>
{
    public Guid? SessionId { get; set; }
    public Guid? UserId { get; set; }
}
