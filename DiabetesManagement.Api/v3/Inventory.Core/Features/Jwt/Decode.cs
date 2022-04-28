using Inventory.Contracts;
using Inventory.Features.Jwt;
using MediatR;

namespace Inventory.Core.Features.Jwt;

public class Decode : IRequestHandler<DecodeRequest, IDictionary<string, string>>
{
    private readonly IJwtProvider jwtProvider;

    public Decode(IJwtProvider jwtProvider)
    {
        this.jwtProvider = jwtProvider;
    }

    public async Task<IDictionary<string, string>> Handle(DecodeRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return jwtProvider.Extract(request.Token!, request.Parameters ?? jwtProvider.DefaultTokenValidationParameters);
    }
}
