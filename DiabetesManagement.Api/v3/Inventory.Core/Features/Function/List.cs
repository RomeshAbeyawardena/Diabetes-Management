using Inventory.Features.Function;
using LinqKit;
using MediatR;

namespace Inventory.Core.Features.Function;

public class List : IRequestHandler<ListRequest, IEnumerable<Models.Function?>>
{
    private readonly IFunctionRepository functionRepository;

    public List(IFunctionRepository functionRepository)
    {
        this.functionRepository = functionRepository;
    }

    public async Task<IEnumerable<Models.Function?>> Handle(ListRequest request, CancellationToken cancellationToken)
    {
        var results = await functionRepository.Get(request, cancellationToken);

        results.ForEach(functionRepository.Decrypt!);
        return results;
    }
}
