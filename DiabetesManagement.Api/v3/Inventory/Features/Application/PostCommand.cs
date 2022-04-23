using Inventory.Attributes;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Features.Application;

[RequiresClaims(Permissions.Application_Edit)]
public class PostCommand : IRequest<Models.Application>
{
    public string? AccessToken { get; set; }
    public string? Claims { get; set; }
    [Required]
    public string? DisplayName { get; set; }
    public TimeSpan? Expires { get; set; }
    [Required]
    public string? Intent { get; set; }
    [Required]
    public string? Name { get; set; }
    public Guid? UserId { get; set; }
}
