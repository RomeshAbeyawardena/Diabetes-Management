using Inventory.Contracts;
using Inventory.Features.ApplicationInstance;
using Inventory.Persistence.Base;

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

        public ApplicationInstanceRepository(IDbContextProvider dbContextProvider, IClockProvider clockProvider) : base(dbContextProvider)
        {
            this.clockProvider = clockProvider;
        }

        public async Task<Models.ApplicationInstance> Save(SaveCommand saveCommand, CancellationToken cancellationToken)
        {
            if(saveCommand == null || saveCommand.ApplicationInstance == null)
            {
                throw new NullReferenceException();
            }

            await Save(saveCommand.ApplicationInstance, cancellationToken);

            if (saveCommand.CommitChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }

            return saveCommand.ApplicationInstance;
        }
    }
}
