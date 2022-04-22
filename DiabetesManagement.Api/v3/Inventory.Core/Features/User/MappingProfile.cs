using AutoMapper;
using DiabetesManagement.Features.User;

namespace DiabetesManagement.Core.Features.User;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PostCommand, GetRequest>();
    }
}
