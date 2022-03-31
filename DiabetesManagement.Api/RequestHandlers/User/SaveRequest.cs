using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Api.RequestHandlers.User
{
    public class SaveRequest : IRequest<Shared.Models.User>
    {
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
