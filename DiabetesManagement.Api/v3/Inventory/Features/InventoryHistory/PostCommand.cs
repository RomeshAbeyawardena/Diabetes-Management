using Inventory.Attributes;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Features.InventoryHistory;

[RequiresClaims(Permissions.InventoryHistory_Edit)]
public class PostCommand : IRequest<Models.InventoryHistory>
{
    [Required]
    public string? Key { get; set; }

    [Required]
    public Guid? UserId { get; set; }

    [Required]
    public string? Type { get; set; }

    [Required]
    public string? Items { get; set; }
}
