using AutoMapper;
using Ledger.Features.Account;

namespace Ledger.Persistence.MappingProfiles;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<GetRequest, Models.Account>();
    }
}
