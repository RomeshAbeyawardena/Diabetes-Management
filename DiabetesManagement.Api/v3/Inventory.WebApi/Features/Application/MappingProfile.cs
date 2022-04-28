using AutoMapper;

namespace Inventory.WebApi.Features.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Models.InventoryHistory, Models.Version>()
            .ForMember(m => m.Value, opt => opt.MapFrom(m => m.Version));
    }
}
