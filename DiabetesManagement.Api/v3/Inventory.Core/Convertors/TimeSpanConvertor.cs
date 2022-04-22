using DiabetesManagement.Attributes;
using DiabetesManagement.Contracts;

namespace DiabetesManagement.Core.Convertors;

[RegisterService]
public class TimeSpanConvertor : IConvertor
{
    private TimeSpan? timeSpan;
    public bool CanConvert(Type type, object value)
    {
        if(type != typeof(TimeSpan) && type != typeof(TimeSpan?))
        {
            return false;
        }

        if(TimeSpan.TryParse(value.ToString(), out var timeSpan)) 
        {
            this.timeSpan = timeSpan;
            return true;
        }

        return false;
    }

    public object? Convert()
    {
        return timeSpan;
    }
}
