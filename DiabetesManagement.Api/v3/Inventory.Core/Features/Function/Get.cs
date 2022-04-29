using Inventory.Features.Function;
using MediatR;

namespace Inventory.Core.Features.Function
{
    public class Get : IRequestHandler<GetRequest, Models.Function?>
    {
        private readonly IFunctionRepository functionRepository;

        public Get(IFunctionRepository functionRepository)
        {
            this.functionRepository = functionRepository;
        }

        public async Task<Models.Function?> Handle(GetRequest request, CancellationToken cancellationToken)
        {
            return await functionRepository.Get(request, cancellationToken);
        }
    }
}
