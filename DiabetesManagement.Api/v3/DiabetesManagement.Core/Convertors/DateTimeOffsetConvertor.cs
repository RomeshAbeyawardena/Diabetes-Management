using DiabetesManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Core.Convertors;

public class DateTimeOffsetConvertor : IConvertor
{
    private DateTimeOffset? value;
    public bool CanConvert(Type type, object value)
    {
        if(type != typeof(DateTimeOffset))
        {
            return false;
        }

        value DateTimeOffset.ParseExact(value.ToString(), "");
    }

    public object? Convert()
    {
        throw new NotImplementedException();
    }
}
