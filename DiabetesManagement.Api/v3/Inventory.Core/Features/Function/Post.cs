using Inventory.Features.Function;
using MediatR;

namespace Inventory.Core.Features.Function;

public class Post : IRequestHandler<PostCommand, Models.Function>
{
    private readonly IFunctionRepository functionRepository;

    public Post(IFunctionRepository functionRepository)
    {
        this.functionRepository = functionRepository;
    }

    public Task<Models.Function> Handle(PostCommand request, CancellationToken cancellationToken)
    {
        return functionRepository.Save(new SaveCommand {
            Function = new Models.Function
            {
                AccessToken = request.AccessToken,
                Name = request.Name,
                Path = request.Path
            },
            CommitChanges = true
        }, cancellationToken);
    }
}
