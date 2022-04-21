using DiabetesManagement.Features.AccessToken;
using MediatR;

namespace DiabetesManagement.Core.Features.AccessToken;

public class Get : IRequestHandler<GetRequest, Models.AccessToken>
{
    private readonly IAccessTokenRepository accessTokenRepository;

    public Get(IAccessTokenRepository accessTokenRepository)
    {
        this.accessTokenRepository = accessTokenRepository;
    }

    public Task<Models.AccessToken> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        return accessTokenRepository.Get(request, cancellationToken);
    }
}
