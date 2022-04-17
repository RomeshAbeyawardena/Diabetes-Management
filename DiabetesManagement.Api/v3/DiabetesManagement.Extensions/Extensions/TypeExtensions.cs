namespace DiabetesManagement.Extensions.Extensions;

public static class TypeExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static object GetDefaultValue(this Type t)
    {
        return t.IsValueType && Nullable.GetUnderlyingType(t) == null
            ? Activator.CreateInstance(t)!
            : null!;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static object GetDefaultValue(this object value)
    {
        return value.GetType().GetDefaultValue();
    }

    public static bool IsDefaultValue(this object value)
    {
        return value.GetDefaultValue().Equals(value);
    }
}
