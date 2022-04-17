using DiabetesManagement.Contracts;
using DiabetesManagement.Core.Base;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Features.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DiabetesManagement.Core.Features.User
{
    public class UserRepository : InventoryDbRepositoryBase<Models.User>, IUserRepository
    {
        private readonly IClockProvider clockProvider;
        private readonly ApplicationSettings applicationSettings;
        private bool prepareEncryptedFields = false;
        private void PrepareEncrpytedFields(Models.User user)
        {
            user.DisplayName = user.DisplayName!.Encrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out string str);
            user.DisplayNameCaseSignature = str;
            user.EmailAddress = user.EmailAddress!.Encrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out str);
            user.EmailAddressCaseSignature = str;
            user.Password = user.Password!.Hash(applicationSettings.HashAlgorithm!, applicationSettings.ConfidentialServerKey!);
        }

        private void DecryptFields(Models.User user)
        {
            user.EmailAddress = user.EmailAddress!.Decrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes, applicationSettings.ServerInitialVectorBytes, user.EmailAddressCaseSignature);
            user.DisplayName = user.DisplayName!.Decrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, user.DisplayNameCaseSignature);
        }

        protected override Task<bool> Add(Models.User user, CancellationToken cancellationToken)
        {
            PrepareEncrpytedFields(user);
            
            user.Created = clockProvider.Clock.UtcNow;
            user.Hash = user.GetHash();
            return AcceptChanges;
        }

        protected override Task<bool> Update(Models.User user, CancellationToken cancellationToken)
        {
            if (prepareEncryptedFields)
            {
                PrepareEncrpytedFields(user);
            }

            user.Modified = clockProvider.Clock.UtcNow;
            return AcceptChanges;
        }

        public UserRepository(IDbContextProvider context, IClockProvider clockProvider, ApplicationSettings applicationSettings) : base(context)
        {
            this.clockProvider = clockProvider;
            this.applicationSettings = applicationSettings;
        }

        public async Task<Models.User?> GetUser(GetRequest request, CancellationToken cancellationToken)
        {
            if (request.UserId.HasValue)
            {
                return await FindAsync(s => s.UserId == request.UserId.Value, cancellationToken);
            }

            var emailAddress = request.EmailAddress!.Encrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out string str);

            Models.User? foundUser = default;

            if (request.AuthenticateUser)
            {
                var password = request.Password!.Hash(applicationSettings.HashAlgorithm!, applicationSettings.ConfidentialServerKey!);

                foundUser = await Query.AsNoTracking().FirstOrDefaultAsync(u => u.EmailAddress == emailAddress && u.Password == password, cancellationToken);
            }
            else
            {
                foundUser = await Query.AsNoTracking().FirstOrDefaultAsync(u => u.EmailAddress == emailAddress, cancellationToken);
            }


            if (foundUser != null)
            {
                DecryptFields(foundUser);
            }

            return foundUser;
        }

        public async Task<Models.User> SaveUser(SaveCommand command, CancellationToken cancellationToken)
        {
            EntityEntry<Models.User> entityEntry;
           

            if (command.User == null)
            {
                throw new NullReferenceException();
            }

            var user = command.User;
            prepareEncryptedFields = command.PrepareEncryptedFields;
            entityEntry = await Save(user, cancellationToken);

            if (command.CommitChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }

            Detach(entityEntry);

            DecryptFields(user);

            return user;
        }
    }
}
