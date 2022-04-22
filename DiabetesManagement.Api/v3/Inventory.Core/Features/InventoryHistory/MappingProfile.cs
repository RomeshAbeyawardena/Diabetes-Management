using AutoMapper;
using DiabetesManagement.Features.InventoryHistory;
using InventoryFeature = DiabetesManagement.Features.Inventory;

namespace DiabetesManagement.Core.Features.InventoryHistory;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PostCommand, InventoryFeature.GetRequest>()
            .ForMember(m => m.Intent, opt => opt.MapFrom(m => m.Key))
            .ForMember(m => m.Subject, opt => opt.MapFrom(m => m.Type));
    }
}
