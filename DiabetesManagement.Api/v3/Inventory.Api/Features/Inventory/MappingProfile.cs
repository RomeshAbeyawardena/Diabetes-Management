using AutoMapper;

namespace Inventory.Api.Features.Inventory;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Models.InventoryHistory, Models.Version>()
            .ForMember(m => m.Value, opt => opt.MapFrom(m => m.Version));
    }
}
