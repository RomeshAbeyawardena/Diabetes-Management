﻿using Inventory.Contracts;
using Ledger.Contracts;

namespace Ledger.Features.Ledger;

public interface ILedgerRepository : IRepository<ILedgerDbContext, Models.Ledger>
{
    Task<IEnumerable<Models.Ledger>> Get(GetRequest request, CancellationToken cancellationToken);
    Task<Models.Ledger> SaveLedger(SaveCommand command, CancellationToken cancellationToken);
}
