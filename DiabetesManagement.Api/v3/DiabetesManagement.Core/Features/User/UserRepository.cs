using DiabetesManagement.Contracts;
using DiabetesManagement.Core.Base;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Features.User;
using Microsoft.EntityFrameworkCore;

namespace DiabetesManagement.Core.Features.User
{
    public class UserRepository : InventoryDbRepositoryBase<Models.User>, IUserRepository
    {
        private readonly ApplicationSettings applicationSettings;

        public UserRepository(InventoryDbContext context, ApplicationSettings applicationSettings) : base(context)
        {
            this.applicationSettings = applicationSettings;
        }

        public async Task<Models.User?> GetUser(GetRequest request, CancellationToken cancellationToken)
        {
            if (request.UserId.HasValue)
            {
                return await DbSet.FindAsync(new object[] { request.UserId.Value }, cancellationToken: cancellationToken);
            }

            var emailAddress = request.EmailAddress!.Encrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out string str);

            Models.User? foundUser = default;

            if (request.AuthenticateUser)
            {
                var password = request.Password!.Hash(applicationSettings.HashAlgorithm!, applicationSettings.ConfidentialServerKey!);

                foundUser = await DbSet.AsNoTracking().FirstOrDefaultAsync(u => u.EmailAddress == emailAddress && u.Password == password, cancellationToken);
            }
            else
            {
                foundUser = await DbSet.AsNoTracking().FirstOrDefaultAsync(u => u.EmailAddress == emailAddress, cancellationToken);
            }


            if (foundUser != null)
            {
                foundUser.EmailAddress = foundUser.EmailAddress!.Decrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes, applicationSettings.ServerInitialVectorBytes, foundUser.EmailAddressCaseSignature);
                foundUser.DisplayName = foundUser.DisplayName!.Decrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, foundUser.DisplayNameCaseSignature);
            }

            return foundUser;
        }

        public async Task<Models.User> SaveUser(SaveCommand command, CancellationToken cancellationToken)
        {
            if (command.User == null)
            {
                throw new NullReferenceException();
            }

            var user = command.User;
            user.DisplayName = user.DisplayName!.Encrypt(applicationSettings.Algorithm!, applicationSettings.PersonalDataServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out string str);
            user.DisplayNameCaseSignature = str;
            user.EmailAddress = user.EmailAddress!.Encrypt(applicationSettings.Algorithm!, applicationSettings.ConfidentialServerKeyBytes, applicationSettings.ServerInitialVectorBytes, out str);
            user.EmailAddressCaseSignature = str;

            user.Created = DateTimeOffset.UtcNow;
            user.Password = user.Password!.Hash(applicationSettings.HashAlgorithm!, applicationSettings.ConfidentialServerKey!);
            user.Hash = user.GetHash();

            DbSet.Add(user);

            if (command.CommitChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }

            return user;
        }
    }
}
