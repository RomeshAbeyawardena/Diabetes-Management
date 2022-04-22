using DiabetesManagement.Attributes;
using DiabetesManagement.Contracts;
using System.Globalization;

namespace DiabetesManagement.Core.Convertors;

[RegisterService]
public class DateTimeOffsetConvertor : IConvertor
{
    private DateTimeOffset? value;
    private bool isDate;
    public bool CanConvert(Type type, object value)
    {
        isDate = type == typeof(DateTime) || type == typeof(DateTime?);

        if (type != typeof(DateTimeOffset) && type != typeof(DateTimeOffset?) && !isDate)
        {
            return false;
        }

        //2022-04-12T20:14:43.8375256+00:00
        if (DateTimeOffset.TryParseExact(value.ToString(), "yyyy-MM-ddTHH:mm:ss.fffffffzzz", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
        {
            this.value = result;
            return true;
        }
        return false;
    }

    public object? Convert()
    {
        if (isDate && value.HasValue)
        {
            return value.Value.DateTime;
        }

        return value;
    }
}
