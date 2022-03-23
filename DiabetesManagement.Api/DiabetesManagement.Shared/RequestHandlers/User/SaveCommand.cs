using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Shared.RequestHandlers.User
{
    public class SaveCommand : IRequest<Guid>
    {
        public Models.User? User { get; set; }
    }
}
