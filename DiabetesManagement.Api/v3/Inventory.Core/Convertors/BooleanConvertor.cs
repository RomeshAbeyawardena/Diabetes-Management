using DiabetesManagement.Attributes;
using DiabetesManagement.Contracts;

namespace DiabetesManagement.Core.Convertors;

[RegisterService]
public class BooleanConvertor : IConvertor
{
    private bool? result;
    public bool CanConvert(Type type, object value)
    {
        if(type != typeof(bool) && type != typeof(bool?))
        {
            return false;
        }

        if(bool.TryParse(value.ToString(), out bool res))
        {
            result = res;
            return true;
        }

        return false;
    }

    public object? Convert()
    {
        return result;
    }
}
