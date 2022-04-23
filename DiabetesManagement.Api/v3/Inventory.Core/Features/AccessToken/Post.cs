using Inventory.Contracts;
using Inventory.Features.AccessToken;
using MediatR;
using System.Collections.ObjectModel;

namespace Inventory.Core.Features.AccessToken;

public class Post : IRequestHandler<PostCommand, Models.AccessToken>
{
    private readonly IAccessTokenRepository accessTokenRepository;
    private readonly IClockProvider clockProvider;

    public Post(IAccessTokenRepository accessTokenRepository, IClockProvider clockProvider)
    {
        this.accessTokenRepository = accessTokenRepository;
        this.clockProvider = clockProvider;
    }

    public async Task<Models.AccessToken> Handle(PostCommand request, CancellationToken cancellationToken)
    {
        var accessToken = new Models.AccessToken
        {
            Key = request.Key,
            Value = request.Value,
            Expires = request.Expires,
            ApplicationId = request.ApplicationId,
        };

        accessToken.AccessTokenClaims ??= new Collection<Models.AccessTokenClaim>();

        foreach (var claim in request.Claims!)
        {
            accessToken.AccessTokenClaims.Add(new Models.AccessTokenClaim
            {
                Claim = claim,
                Created = clockProvider.Clock.UtcNow,
                Expires = clockProvider.Clock.UtcNow.AddDays(365)
            });
        }

        var result = await accessTokenRepository.Save(new SaveCommand
        {
            AccessToken = accessToken,
            CommitChanges = true
        }, cancellationToken);

        accessTokenRepository.Decrypt(result);

        return result;
    }
}
