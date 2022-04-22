using AutoMapper;
using Inventory.Features.User;

namespace Inventory.Core.Features.User;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PostCommand, GetRequest>();
    }
}
