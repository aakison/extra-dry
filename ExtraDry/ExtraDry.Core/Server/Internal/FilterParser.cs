using ExtraDry.Core;
using Pidgin;
using System.Collections.Generic;
using System.Linq;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace ExtraDry.Server.Internal {

    internal static class FilterParser {

        public static Filter Parse(string filter)
        {
            var parsed = Filters.Parse(filter);
            if(!parsed.Success) {
                throw new DryException($"Invalid filter expression '{filter}' resulted in '{parsed.Error}'", "Unable to apply filter. 0x0F947CB5");
            }
            return parsed.Value;
        }

        private static readonly Parser<char, char> Colon = Char(':');

        private static readonly Parser<char, char> Pipe = Char('|');

        private static readonly Parser<char, char> Quote = Char('"');

        private static readonly Parser<char, char> Underscore = Char('_');

        private static readonly Parser<char, char> Comma = Char(',');

        private static readonly Parser<char, char> ValueCharacter = Token(c => char.IsLetterOrDigit(c) || c == '-' || c == '.' || c == ':');

        private static readonly Parser<char, BoundRule> LeftBracket = Char('[').Select(e => BoundRule.Inclusive);

        private static readonly Parser<char, BoundRule> RightBracket = Char(']').Select(e => BoundRule.Inclusive);

        private static readonly Parser<char, BoundRule> LeftParen = Char('(').Select(e => BoundRule.Exclusive);

        private static readonly Parser<char, BoundRule> RightParen = Char(')').Select(e => BoundRule.Exclusive);

        private static readonly Parser<char, char> IdentifierStartChar = Letter.Or(Underscore);

        private static readonly Parser<char, string> Identifier = Map(
            (first, rest) => $"{first}{rest}",
            IdentifierStartChar, LetterOrDigit.ManyString()
        );

        private static readonly Parser<char, string> QuotedValue = Token(c => c != '"').ManyString().Between(Quote);

        private static readonly Parser<char, string> MandatoryValue = QuotedValue.Or(ValueCharacter.AtLeastOnceString());

        private static readonly Parser<char, string> OptionalValue = QuotedValue.Or(ValueCharacter.ManyString());

        private static readonly Parser<char, IEnumerable<string>> ValueList = MandatoryValue.Separated(Pipe);

        private static readonly Parser<char, BoundRule> LowerBound = LeftBracket.Or(LeftParen);

        private static readonly Parser<char, BoundRule> UpperBound = RightBracket.Or(RightParen);

        private static readonly Parser<char, FilterRule> BetweenExpression = Map(
            (lowerBound, lowerValue, _, upperValue, upperBound) => new FilterRule("", lowerBound, lowerValue, upperBound, upperValue),
            LowerBound, OptionalValue, Comma, OptionalValue, UpperBound
        );

        private static readonly Parser<char, FilterRule> MatchExpression = ValueList.Select(e => new FilterRule("", e));

        private static readonly Parser<char, FilterRule> ValueExpression = BetweenExpression.Or(MatchExpression);

        private static readonly Parser<char, FilterRule> FilterRule = Map(
            (n, _, v) => new FilterRule(n, v),
            Identifier, Colon, ValueExpression
        );

        private static readonly Parser<char, IEnumerable<FilterRule>> CompositeFilterRule = FilterRule.Separated(Whitespace);

        private static readonly Parser<char, Filter> Filters = CompositeFilterRule.Select(e => new Filter(e)).Before(End);

    }

}
