using DiabetesManagement.Contracts;

namespace DiabetesManagement.Core.Convertors;

public class IntConvertor : IConvertor
{
    private int? value;
    public bool CanConvert(Type type, object value)
    {
        if (type != typeof(int) && type != typeof(int?))
        {
            return false;
        }

        if (int.TryParse(value.ToString(), out var result))
        {
            this.value = result;
            return true;
        }

        return false;
    }

    public object? Convert()
    {
        return value;
    }
}
