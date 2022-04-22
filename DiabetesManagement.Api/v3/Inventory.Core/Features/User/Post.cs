
using AutoMapper;
using Inventory.Features.User;
using MediatR;

namespace Inventory.Core.Features.User;

public class Post : IRequestHandler<PostCommand, Models.User>
{
    private readonly IMapper mapper;
    private readonly IUserRepository userRepository;

    public Post(IMapper mapper, IUserRepository userRepository)
    {
        this.mapper = mapper;
        this.userRepository = userRepository;
    }

    public async Task<Models.User> Handle(PostCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUser(mapper.Map<GetRequest>(request), cancellationToken);

        if (user != null)
        {
            throw new InvalidOperationException("User already exists");
        }

        return await userRepository.SaveUser(new SaveCommand
        {
            User = new Models.User
            {
                DisplayName = request.DisplayName,
                EmailAddress = request.EmailAddress,
                Password = request.Password,
            },
            CommitChanges = true
        }, cancellationToken);
    }
}
