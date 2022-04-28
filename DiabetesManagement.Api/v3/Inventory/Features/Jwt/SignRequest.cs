using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.Features.Jwt;

public class SignRequest : IRequest<string>
{
    public IDictionary<string, object>? Dictionary { get; set; }
    public TokenValidationParameters? Parameters { get; set; }
}
