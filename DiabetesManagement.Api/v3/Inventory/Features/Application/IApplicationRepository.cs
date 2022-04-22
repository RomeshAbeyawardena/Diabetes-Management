namespace DiabetesManagement.Features.Application;

public interface IApplicationRepository
{
    Task<Models.Application> Save(SaveCommand saveCommand, CancellationToken cancellationToken);
}
