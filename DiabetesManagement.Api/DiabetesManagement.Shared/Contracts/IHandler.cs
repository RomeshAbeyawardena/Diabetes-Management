using Microsoft.Extensions.Logging;
using System.Data;

namespace DiabetesManagement.Shared.Contracts
{
    public interface IHandler : IDisposable
    {
        ILogger SetLogger { set; }
        IDbTransaction SetTransaction { set; }
        IObservable<ILogger> LoggerChanged { get; }
        IObservable<IDbTransaction> DbTransactionChanged { get; }
    }
}
