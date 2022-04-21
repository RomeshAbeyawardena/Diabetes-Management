using DiabetesManagement.Attributes;
using MediatR;

namespace DiabetesManagement.Features.AccessToken
{
    [RequiresClaims(Permissions.Anonymous_Access, Permissions.AccessToken_View)]
    public class GetRequest : IRequest<Models.AccessToken>
    {
        public Guid Key { get; set; }
        public string? AccessToken { get; set; }
    }
}
