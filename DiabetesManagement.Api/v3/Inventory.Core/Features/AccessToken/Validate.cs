using JwtFeature = Inventory.Features.Jwt;
using Inventory.Features.AccessToken;
using MediatR;

namespace Inventory.Core.Features.AccessToken;

public class Validate : IRequestHandler<ValidateRequest, IDictionary<string, string>>
{
    private readonly IMediator mediator;

    public Validate(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<IDictionary<string, string>> Handle(ValidateRequest request, CancellationToken cancellationToken)
    {
        return await mediator.Send(new JwtFeature.DecodeRequest { Token = request.Token }, cancellationToken);
    }
}
