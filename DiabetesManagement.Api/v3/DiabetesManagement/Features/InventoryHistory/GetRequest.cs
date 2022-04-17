using DiabetesManagement.Attributes;
using MediatR;

namespace DiabetesManagement.Features.InventoryHistory;

[RequiresClaims(Permissions.InventoryHistory_View)]
public class GetRequest : IRequest<IEnumerable<Models.InventoryHistory>>
{

}
