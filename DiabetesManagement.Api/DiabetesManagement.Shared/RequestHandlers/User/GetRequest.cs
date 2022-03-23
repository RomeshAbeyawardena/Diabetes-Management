using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Shared.RequestHandlers.User
{
    public class GetRequest : IRequest<Models.User>
    {
        public Guid? UserId { get; set; }
        public string? Username { get; set; }
        public string? EmailAddress { get; set; }
    }
}
