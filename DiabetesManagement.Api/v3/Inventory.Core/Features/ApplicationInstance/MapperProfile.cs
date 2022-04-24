using AutoMapper;
using Inventory.Features.ApplicationInstance;

namespace Inventory.Core.Features.ApplicationInstance;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<PutCommand, SaveCommand>();
    }
}
