using AutoMapper;
using Inventory.Features.Session;

namespace Inventory.Core.Features.Session;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PostCommand, GetRequest>();
    }
}
