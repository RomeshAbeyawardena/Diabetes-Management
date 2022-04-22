using DiabetesManagement.Attributes;
using MediatR;

namespace DiabetesManagement.Features.AccessToken;
[RequiresClaims(Permissions.Anonymous_Access)]
public class ValidateRequest : IRequest<IDictionary<string, string>>
{
    public string? Token { get; set; }
}
