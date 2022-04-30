﻿using MediatR;
using Ledger.Features.Ledger;

namespace Ledger.Core.Features.Ledger;

public class Post : IRequestHandler<GetRequest, IEnumerable<Models.Ledger>>
{
    public Task<IEnumerable<Models.Ledger>> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
