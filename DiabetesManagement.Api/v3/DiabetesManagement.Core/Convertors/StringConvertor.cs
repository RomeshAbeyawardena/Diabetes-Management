using DiabetesManagement.Contracts;

namespace DiabetesManagement.Core.Convertors;

public class StringConvertor : IConvertor
{
    private string value;
    public bool CanConvert(Type type, object value)
    {
        if (type != typeof(string))
        {
            return false;
        }

        this.value = value.ToString()!;
        return true;
    }

    public object? Convert()
    {
        return this.value;
    }
}
