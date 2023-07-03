using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ExtraDry.Highlight;

public class HtmlEngine : Engine
{
    private const string StyleSpanFormat = "<span style=\"{0}\">{1}</span>";
    private const string ClassSpanFormat = "<span class=\"{0}\">{1}</span>";

    public bool UseCss { get; set; }

    protected override string PreHighlight(Definition definition, string input)
    {
        return HttpUtility.HtmlEncode(input);
    }

    protected override string PostHighlight(Definition definition, string input)
    {
        if (UseCss)
        {
            var cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, null);

            return String.Format(ClassSpanFormat, cssClassName, input);
        }
        else
        {
            var cssStyle = HtmlEngineHelper.CreatePatternStyle(definition.Style);

            return String.Format(StyleSpanFormat, cssStyle, input);
        }
    }

    protected override string ProcessBlockPatternMatch(Definition definition, BlockPattern pattern, Match match)
    {
        if (!UseCss) {
            var patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.Style);

            return String.Format(StyleSpanFormat, patternStyle, match.Value);
        }

        var cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name);

        return String.Format(ClassSpanFormat, cssClassName, match.Value);
    }

    protected override string ProcessMarkupPatternMatch(Definition definition, MarkupPattern pattern, Match match)
    {
        if (definition == null) {
            throw new ArgumentNullException(nameof(definition));
        }
        if (pattern == null) {
            throw new ArgumentNullException(nameof(pattern));
        }
        if (match == null) {
            throw new ArgumentNullException(nameof(match));
        }

        var result = new StringBuilder();
        if (!UseCss) {
            var patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.BracketColors, pattern.Style.Font);
            result.AppendFormat(StyleSpanFormat, patternStyle, match.Groups["openTag"].Value);

            result.Append(match.Groups["ws1"].Value);

            patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.Style);
            result.AppendFormat(StyleSpanFormat, patternStyle, match.Groups["tagName"].Value);

            if (pattern.HighlightAttributes) {
                var highlightedAttributes = ProcessMarkupPatternAttributeMatches(definition, pattern, match);
                result.Append(highlightedAttributes);
            }

            result.Append(match.Groups["ws5"].Value);

            patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.BracketColors, pattern.Style.Font);
            result.AppendFormat(StyleSpanFormat, patternStyle, match.Groups["closeTag"].Value);
        }
        else {
            var cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name + "Bracket");
            result.AppendFormat(ClassSpanFormat, cssClassName, match.Groups["openTag"].Value);

            result.Append(match.Groups["ws1"].Value);

            cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name + "TagName");
            result.AppendFormat(ClassSpanFormat, cssClassName, match.Groups["tagName"].Value);

            if (pattern.HighlightAttributes) {
                var highlightedAttributes = ProcessMarkupPatternAttributeMatches(definition, pattern, match);
                result.Append(highlightedAttributes);
            }

            result.Append(match.Groups["ws5"].Value);

            cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name + "Bracket");
            result.AppendFormat(ClassSpanFormat, cssClassName, match.Groups["closeTag"].Value);
        }

        return result.ToString();
    }

    protected override string ProcessWordPatternMatch(Definition definition, WordPattern pattern, Match match)
    {
        if (!UseCss) {
            var patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.Style);

            return String.Format(StyleSpanFormat, patternStyle, match.Value);
        }

        var cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name);

        return String.Format(ClassSpanFormat, cssClassName, match.Value);
    }

    private string ProcessMarkupPatternAttributeMatches(Definition definition, MarkupPattern pattern, Match match)
    {
        var result = new StringBuilder();

        for (var i = 0; i < match.Groups["attribute"].Captures.Count; i++) {
            result.Append(match.Groups["ws2"].Captures[i].Value);
            if (!UseCss) {
                var patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.AttributeNameColors, pattern.Style.Font);
                result.AppendFormat(StyleSpanFormat, patternStyle, match.Groups["attribName"].Captures[i].Value);

                if (String.IsNullOrWhiteSpace(match.Groups["attribValue"].Captures[i].Value)) {
                    continue;
                }

                patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.AttributeValueColors, pattern.Style.Font);
                result.AppendFormat(StyleSpanFormat, patternStyle, match.Groups["attribValue"].Captures[i].Value);
            }
            else {
                var cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name + "AttributeName");
                result.AppendFormat(ClassSpanFormat, cssClassName, match.Groups["attribName"].Captures[i].Value);

                if (String.IsNullOrWhiteSpace(match.Groups["attribValue"].Captures[i].Value)) {
                    continue;
                }

                cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name + "AttributeValue");
                result.AppendFormat(ClassSpanFormat, cssClassName, match.Groups["attribValue"].Captures[i].Value);
            }
        }

        return result.ToString();
    }
}