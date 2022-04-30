using AutoMapper;
using Ledger.Features.Ledger;

namespace Ledger.Persistence.MappingProfiles;

public class LedgerProfile : Profile
{
    public LedgerProfile()
    {
        CreateMap<GetRequest, Models.Account>()
            .ForMember(m => m.Reference, opt => opt.MapFrom(m => m.AccountReference));

        CreateMap<GetRequest, Models.Ledger>()
            .ForMember(m => m.Reference, opt => opt.MapFrom(m => m.LedgerReference));
    }
}
