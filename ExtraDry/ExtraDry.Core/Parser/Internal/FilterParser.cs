using Pidgin;
using static Pidgin.Parser<char>;
using static Pidgin.Parser;

namespace ExtraDry.Core.Parser.Internal;

public static class FilterParser
{
    public static Filter Parse(string filter)
    {
        var parsed = Filters.Parse(filter);
        return parsed.Success
            ? parsed.Value
            : throw new DryException($"Invalid filter expression '{filter}' resulted in '{parsed.Error}'", "Unable to apply filter. 0x0F947CB5");
    }

    private static readonly Parser<char, char> Colon = Char(':');

    private static readonly Parser<char, char> Pipe = Char('|');

    private static readonly Parser<char, char> Quote = Char('"');

    private static readonly Parser<char, char> Comma = Char(',');

    private static readonly Parser<char, char> ValueCharacter = Token(c => char.IsLetterOrDigit(c) || c == '-' || c == '.');

    private static readonly Parser<char, char> ExtendedValueCharacter = Token(c => char.IsLetterOrDigit(c) || c == '-' || c == '.' || c == ':');

    private static readonly Parser<char, char> IdentifierStartChar = Token(c => char.IsLetter(c) || c == '_');

    private static readonly Parser<char, BoundRule> LeftBracket = Char('[').Select(e => BoundRule.Inclusive);

    private static readonly Parser<char, BoundRule> RightBracket = Char(']').Select(e => BoundRule.Inclusive);

    private static readonly Parser<char, BoundRule> LeftParen = Char('(').Select(e => BoundRule.Exclusive);

    private static readonly Parser<char, BoundRule> RightParen = Char(')').Select(e => BoundRule.Exclusive);

    private static readonly Parser<char, string> Identifier = Map(
        (first, rest) => $"{first}{rest}",
        IdentifierStartChar, LetterOrDigit.ManyString()
    );

    private static readonly Parser<char, string> QuotedValue = Token(c => c != '"').ManyString().Between(Quote);

    private static readonly Parser<char, string> MandatoryValue = QuotedValue.Or(ValueCharacter.AtLeastOnceString());

    private static readonly Parser<char, string> OptionalExtendedValue = QuotedValue.Or(ExtendedValueCharacter.ManyString());

    private static readonly Parser<char, IEnumerable<string>> ValueList = MandatoryValue.Separated(Pipe);

    private static readonly Parser<char, BoundRule> LowerBound = LeftBracket.Or(LeftParen);

    private static readonly Parser<char, BoundRule> UpperBound = RightBracket.Or(RightParen);

    private static readonly Parser<char, FilterRule> BetweenExpression = Map(
        (lowerBound, lowerValue, _, upperValue, upperBound) => new FilterRule("", lowerBound, lowerValue, upperBound, upperValue),
        LowerBound, OptionalExtendedValue, Comma, OptionalExtendedValue, UpperBound
    );

    private static readonly Parser<char, FilterRule> MatchExpression = ValueList.Select(e => new FilterRule("", e));

    private static readonly Parser<char, FilterRule> ValueExpression = BetweenExpression.Or(MatchExpression);

    private static readonly Parser<char, FilterRule> NamedFilterRule = Map(
        (n, _, v) => new FilterRule(n, v),
        Identifier, Colon, ValueExpression
    );

    private static readonly Parser<char, FilterRule> UnnamedFilterRule = Map(
        (v) => new FilterRule("*", v),
        MandatoryValue
    );

    private static readonly Parser<char, FilterRule> FilterRule = Try(NamedFilterRule).Or(UnnamedFilterRule);

    private static readonly Parser<char, IEnumerable<FilterRule>> CompositeFilterRule = FilterRule.Separated(Whitespace);

    private static readonly Parser<char, Filter> Filters = CompositeFilterRule.Before(End).Select(e => new Filter(e));
}
