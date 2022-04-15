using DiabetesManagement.Attributes;
using DiabetesManagement.Contracts;
using Microsoft.Extensions.Internal;

namespace DiabetesManagement.Core.Defaults;

[RegisterService]
public class DefaultClockProvider : IClockProvider
{
    public DefaultClockProvider()
    {
        Clock = new SystemClock();
    }

    public ISystemClock Clock { get; }
}
