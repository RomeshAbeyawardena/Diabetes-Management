using Microsoft.Extensions.Internal;

namespace Inventory.Contracts;

public interface IClockProvider
{
    ISystemClock Clock { get; }
}
