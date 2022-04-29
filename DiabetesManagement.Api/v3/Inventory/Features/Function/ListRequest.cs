using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.Function;

[RequiresClaims(Permissions.Function_View)]
public class ListRequest : IRequest<IEnumerable<Models.Function>>
{

}
