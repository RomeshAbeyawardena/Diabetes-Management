using Inventory.Features.Function;
using MediatR;

namespace Inventory.Core.Features.Function
{
    public class List : IRequestHandler<ListRequest, IEnumerable<Models.Function?>>
    {
        private readonly IFunctionRepository functionRepository;

        public List(IFunctionRepository functionRepository)
        {
            this.functionRepository = functionRepository;
        }

        public Task<IEnumerable<Models.Function?>> Handle(ListRequest request, CancellationToken cancellationToken)
        {
            return functionRepository.Get(request, cancellationToken);
        }
    }
}
