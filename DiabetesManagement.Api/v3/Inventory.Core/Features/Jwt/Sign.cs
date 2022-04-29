using Inventory.Contracts;
using Inventory.Features.Jwt;
using MediatR;

namespace Inventory.Core.Features.Jwt;

public class Sign : IRequestHandler<SignRequest, string>
{
    private readonly IJwtProvider jwtProvider;

    public Sign(IJwtProvider jwtProvider)
    {
        this.jwtProvider = jwtProvider;
    }

    public async Task<string> Handle(SignRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        if(request.Dictionary == null)
        {
            throw new NullReferenceException();
        }

        return jwtProvider.BuildToken(request.Dictionary.ToDictionary(s => s.Key, s => s.Value)!, request.Parameters ?? jwtProvider.DefaultTokenValidationParameters);
    }
}
