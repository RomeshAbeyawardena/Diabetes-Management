using Inventory.Attributes;
using Inventory.Contracts;
using Microsoft.Extensions.Internal;

namespace Inventory.Core.Defaults;

[RegisterService]
public class DefaultClockProvider : IClockProvider
{
    public DefaultClockProvider()
    {
        Clock = new SystemClock();
    }

    public ISystemClock Clock { get; }
}
