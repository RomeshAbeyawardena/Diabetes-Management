using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Shared.RequestHandlers.User
{
    public class GetRequest : IRequest<Models.User>
    {
        public Guid? UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? Password { get; set; }
        public string? EmailAddress { get; set; }
    }
}
