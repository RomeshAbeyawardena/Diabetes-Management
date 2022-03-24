using DiabetesManagement.Shared.Contracts;

namespace DiabetesManagement.Shared.RequestHandlers.ApiToken
{
    public class SaveCommand : IRequest<Guid>
    {
        public Models.ApiToken? ApiToken { get; set; }
    }
}
