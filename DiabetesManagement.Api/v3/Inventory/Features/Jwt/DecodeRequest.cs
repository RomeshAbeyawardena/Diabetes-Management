using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.Features.Jwt;

public class DecodeRequest : IRequest<IDictionary<string, string>>
{
    public string? Token { get; set; }
    
    public TokenValidationParameters? Parameters { get; set; }
}
