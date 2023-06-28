using System;

namespace ExtraDry.Highlight;

internal static class StringExtensions
{
    public static float ToSingle(this string input, float defaultValue)
    {
        if (float.TryParse(input, out var result)) {
            return result;
        }
        return defaultValue;
    }
}
