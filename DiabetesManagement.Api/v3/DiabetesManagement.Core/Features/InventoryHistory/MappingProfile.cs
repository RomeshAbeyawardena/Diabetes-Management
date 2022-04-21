using AutoMapper;
using DiabetesManagement.Features.InventoryHistory;

namespace DiabetesManagement.Core.Features.InventoryHistory;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PostCommand, GetRequest>();
    }
}
