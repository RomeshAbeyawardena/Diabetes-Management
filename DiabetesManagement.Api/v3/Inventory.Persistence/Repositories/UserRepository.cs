using AutoMapper;
using Inventory;
using Inventory.Contracts;
using Inventory.Extensions;
using Inventory.Features.User;
using Inventory.Persistence.Base;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Repositories
{
    public class UserRepository : InventoryDbRepositoryBase<Models.User>, IUserRepository
    {
        private readonly IMapper mapper;
        private readonly IClockProvider clockProvider;
        private readonly ApplicationSettings applicationSettings;
        private bool prepareEncryptedFields = false;
        
        protected override async Task<bool> Validate(EntityState entityState, Models.User model, CancellationToken cancellationToken)
        {
            if (entityState == EntityState.Added || prepareEncryptedFields)
            {
                Encrypt(model);
            }

            return await (entityState == EntityState.Added
                ? Query.AnyAsync(u => u.EmailAddress == model.EmailAddress, cancellationToken)
                : Query.AnyAsync(u => u.UserId != model.UserId && u.EmailAddress == model.EmailAddress, cancellationToken)) == false;
        }

        protected override Task<bool> Add(Models.User user, CancellationToken cancellationToken)
        {
            user.Created = clockProvider.Clock.UtcNow;
            user.Hash = user.GetHash();
            return AcceptChanges;
        }

        protected override Task<bool> Update(Models.User user, CancellationToken cancellationToken)
        {
            user.Modified = clockProvider.Clock.UtcNow;
            user.Hash = user.GetHash();
            return AcceptChanges;
        }

        public UserRepository(IDbContextProvider context, IMapper mapper, IClockProvider clockProvider, ApplicationSettings applicationSettings) : base(context)
        {
            this.mapper = mapper;
            this.clockProvider = clockProvider;
            this.applicationSettings = applicationSettings;
        }

        public async Task<Models.User?> GetUser(GetRequest request, CancellationToken cancellationToken)
        {
            if (request.UserId.HasValue)
            {
                return await FindAsync(s => s.UserId == request.UserId.Value, cancellationToken);
            }

            var requestUser = mapper.Map<Models.User>(request);
            
            Encrypt(requestUser);

            Models.User? foundUser = default;

            if (request.AuthenticateUser)
            {
                foundUser = await Query.FirstOrDefaultAsync(u => u.EmailAddress == requestUser.EmailAddress && u.Password == requestUser.Password, cancellationToken);
            }
            else
            {
                foundUser = await Query.FirstOrDefaultAsync(u => u.EmailAddress == requestUser.EmailAddress, cancellationToken);
            }

            if (foundUser != null)
            {
                Decrypt(foundUser);
            }

            return foundUser;
        }

        public async Task<Models.User> SaveUser(SaveCommand command, CancellationToken cancellationToken)
        {
            prepareEncryptedFields = command.PrepareEncryptedFields;
            return await base.Save(command, cancellationToken);
        }

        public void Encrypt(Models.User user)
        {
            user.DisplayName = user.DisplayName!.Encrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out string str);
            user.DisplayNameCaseSignature = str;
            user.EmailAddress = user.EmailAddress!.Encrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out str);
            user.EmailAddressCaseSignature = str;
            user.Password = user.Password!.Hash(applicationSettings.HashAlgorithm!, applicationSettings.ConfidentialServerKey!);
        }

        public void Decrypt(Models.User user)
        {
            user.EmailAddress = user.EmailAddress!.Decrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes, applicationSettings.ServerInitialVectorBytes, user.EmailAddressCaseSignature);
            user.DisplayName = user.DisplayName!.Decrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, user.DisplayNameCaseSignature);
        }
    }
}
