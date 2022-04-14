using DiabetesManagement.Features.User;
using MediatR;

namespace DiabetesManagement.Core.Features.User;

public class Get : IRequestHandler<GetRequest, Models.User>
{
    private readonly IUserRepository userRepository;

    public Get(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public Task<Models.User> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        return userRepository.GetUser(request, cancellationToken)!;
    }
}
