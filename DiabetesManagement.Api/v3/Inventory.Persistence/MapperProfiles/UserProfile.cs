using AutoMapper;
using Inventory.Features.User;

namespace Inventory.Persistence.MapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<GetRequest, Models.User>();
    }
}
