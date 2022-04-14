using AutoMapper;
using DiabetesManagement.Features.Session;

namespace DiabetesManagement.Core.Features.Session;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PostCommand, GetRequest>();
    }
}
