using DiabetesManagement.Attributes;
using MediatR;

namespace DiabetesManagement.Features.AccessToken
{
    [RequiresClaims(Permissions.AccessToken_View)]
    public class GetRequest : IRequest<Models.AccessToken>
    {
        public string? AccessToken { get; set; }
    }
}
