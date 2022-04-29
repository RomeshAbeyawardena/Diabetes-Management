using Inventory.Attributes;
using MediatR;

namespace Inventory.Features.Function;

[RequiresClaims(Permissions.Function_View)]
public class ListRequest : IRequest<IEnumerable<Models.Function>>
{
    public int? PageNumber { get; set; }
    public int? TotalItemsPerPage { get; set; }
    public bool? DisplayAll { get; set; }
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
}
