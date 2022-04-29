using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.Features.Jwt;

public class SignRequest : IRequest<string>
{
    public Dictionary<string, System.Text.Json.JsonElement>? Values { get; set; }
    public Dictionary<string, object>? Dictionary { get; set; }
    public TokenValidationParameters? Parameters { get; set; }
}
