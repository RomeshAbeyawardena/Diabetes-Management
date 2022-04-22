using Inventory.Contracts;
using Inventory.Features.AccessToken;
using MediatR;

namespace Inventory.Core.Features.AccessToken;

public class Validate : IRequestHandler<ValidateRequest, IDictionary<string, string>>
{
    private readonly IJwtProvider jwtProvider;

    public Validate(IJwtProvider jwtProvider)
    {
        this.jwtProvider = jwtProvider;
    }

    public async Task<IDictionary<string, string>> Handle(ValidateRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return jwtProvider.Extract(request.Token!, jwtProvider.DefaultTokenValidationParameters);
    }
}
