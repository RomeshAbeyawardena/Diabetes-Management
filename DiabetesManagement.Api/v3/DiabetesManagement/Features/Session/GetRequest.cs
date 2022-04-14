using MediatR;

namespace DiabetesManagement.Features.Session;

public class GetRequest : IRequest<Models.Session>
{
    public Guid? SessionId { get; set; }
    public Guid? UserId { get; set; }
}
