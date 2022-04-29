using AutoMapper;
using Inventory.Features.Function;

namespace Inventory.Persistence.MapperProfiles;

public class FunctionProfile : Profile
{
    public FunctionProfile()
    {
        CreateMap<GetRequest, Models.Function>();
        CreateMap<ListRequest, Models.Function>();
    }
}
