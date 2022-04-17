using MediatR;

namespace DiabetesManagement.Features.AccessToken
{
    public class GetRequest : IRequest<Models.AccessToken>
    {
        public string? AccessToken { get; set; }
    }
}
