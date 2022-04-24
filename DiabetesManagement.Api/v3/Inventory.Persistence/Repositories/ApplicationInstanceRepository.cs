using Inventory.Contracts;
using Inventory.Features.ApplicationInstance;
using Inventory.Persistence.Base;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Repositories
{
    public class ApplicationInstanceRepository : InventoryDbRepositoryBase<Models.ApplicationInstance>, IApplicationInstanceRepository
    {
        private readonly IClockProvider clockProvider;

        protected override Task<bool> Add(Models.ApplicationInstance model, CancellationToken cancellationToken)
        {
            model.Enabled = true;
            model.Created = clockProvider.Clock.UtcNow;
            return AcceptChanges;
        }

        protected override Task<bool> Update(Models.ApplicationInstance model, CancellationToken cancellationToken)
        {
            model.Modified = clockProvider.Clock.UtcNow;
            return AcceptChanges;
        }

        public ApplicationInstanceRepository(IDbContextProvider dbContextProvider, IClockProvider clockProvider) : base(dbContextProvider)
        {
            this.clockProvider = clockProvider;
        }

        public Task<Models.ApplicationInstance> Save(SaveCommand saveCommand, CancellationToken cancellationToken)
        {
            return base.Save(saveCommand, cancellationToken);
        }

        public async Task<IEnumerable<Models.ApplicationInstance>> Get(GetRequest request, CancellationToken cancellationToken)
        {
            var utcNow = clockProvider.Clock.UtcNow;
            return await (request.ApplicationInstanceId.HasValue
                ? Query.Where(a => a.ApplicationInstanceId == request.ApplicationInstanceId.Value && a.Enabled && a.Expires >= utcNow)
                : Query.Where(a => a.ApplicationId == request.ApplicationId && a.Enabled && a.Expires >= utcNow)).ToArrayAsync(cancellationToken);
        }
    }
}
