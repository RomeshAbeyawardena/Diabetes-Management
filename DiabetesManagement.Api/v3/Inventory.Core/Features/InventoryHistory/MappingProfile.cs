using AutoMapper;
using Inventory.Features.InventoryHistory;
using InventoryFeature = Inventory.Features.Inventory;

namespace Inventory.Core.Features.InventoryHistory;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PostCommand, InventoryFeature.GetRequest>()
            .ForMember(m => m.Intent, opt => opt.MapFrom(m => m.Key))
            .ForMember(m => m.Subject, opt => opt.MapFrom(m => m.Type));
    }
}
