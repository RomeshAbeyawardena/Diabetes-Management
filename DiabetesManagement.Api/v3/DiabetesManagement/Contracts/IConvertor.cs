﻿namespace DiabetesManagement.Contracts;

public interface IConvertor
{
    bool CanConvert(Type type, object value);
    object? Convert();
}
