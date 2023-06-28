using System;
using System.Text.RegularExpressions;

namespace ExtraDry.Highlight;

public sealed class BlockPattern : Pattern
{
    public BlockPattern(string name, Style style, string beginsWith, string endsWith, string escapesWith)
        : base(name, style)
    {
        BeginsWith = beginsWith;
        EndsWith = endsWith;
        EscapesWith = escapesWith;
    }
    public string BeginsWith { get; private set; }

    public string EndsWith { get; private set; }

    public string EscapesWith { get; private set; }

    public override string GetRegexPattern()
    {
        if (string.IsNullOrEmpty(EscapesWith)) {
            if (EndsWith.CompareTo(@"\n") == 0) {
                return string.Format(@"{0}[^\n\r]*", Escape(BeginsWith));
            }

            return string.Format(@"{0}[\w\W\s\S]*?{1}", Escape(BeginsWith), Escape(EndsWith));
        }

        return string.Format("{0}(?>{1}.|[^{2}]|.)*?{3}", new object[] { Regex.Escape(BeginsWith), Regex.Escape(EscapesWith.Substring(0, 1)), Regex.Escape(EndsWith.Substring(0, 1)), Regex.Escape(EndsWith) });
    }

    public static string Escape(string str)
    {
        if (str.CompareTo(@"\n") != 0) {
            str = Regex.Escape(str);
        }

        return str;
    }
}
