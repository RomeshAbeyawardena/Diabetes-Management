using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.Features.Jwt;

public class SignRequest : IRequest<string>
{
    public Dictionary<string, object>? Dictionary { get; set; }
    public TokenValidationParameters? Parameters { get; set; }
}
