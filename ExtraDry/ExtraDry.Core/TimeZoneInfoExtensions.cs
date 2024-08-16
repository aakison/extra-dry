using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraDry.Core;

public static class TimeZoneInfoExtensions
{

    public static string DisplayCode(this TimeZoneInfo timeZone)
    {
        var code = timeZone.Id;
        if(code.StartsWith("UTC")) {
            return code;
        }
        var offset = timeZone.BaseUtcOffset;
        var sign = offset.TotalHours > 0 ? "+" : "";
        return $"{code} ({sign}{offset.TotalHours})";
    }

}
