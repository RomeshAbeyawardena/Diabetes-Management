using MediatR;

namespace DiabetesManagement.Features.Application;

public class PostCommand : IRequest<Models.Application>
{
    public string? DisplayName { get; set; }
    public TimeSpan? Expires { get; set; }
    public string? Intent { get; set; }
    public string? Name { get; set; }
    public Guid? UserId { get; set; }
}
