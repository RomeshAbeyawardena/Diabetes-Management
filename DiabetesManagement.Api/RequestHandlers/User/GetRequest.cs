using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Api.RequestHandlers.User
{
    public class GetRequest : IRequest<Shared.Models.User>
    {
        public string EmailAddress { get; set; }
    }
}
