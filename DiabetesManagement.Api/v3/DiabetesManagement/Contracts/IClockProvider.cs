using Microsoft.Extensions.Internal;

namespace DiabetesManagement.Contracts;

public interface IClockProvider
{
    ISystemClock Clock { get; }
}
