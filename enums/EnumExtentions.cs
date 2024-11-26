using System;
using System.Collections;
using System.Collections.Generic;


public static class EnumExtensions
{
    public static T GetNextEnum<T>(this T enumValue) where T : Enum
    {
        T[] values = (T[])Enum.GetValues(enumValue.GetType());
        int index = Array.IndexOf(values, enumValue);
        
        if (index >= 0 && index < values.Length - 1)
        {
            return values[index + 1];
        }
        else
        {
            return values[0]; // or handle the end of the enum as needed
        }
    }
}