using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DiabetesManagement.Features.InventoryHistory
{
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
}
