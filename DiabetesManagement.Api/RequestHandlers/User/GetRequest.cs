using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Api.RequestHandlers.User
{
    public class GetRequest : IRequest<Shared.Models.User>
    {
        public bool AuthenticateUser { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
