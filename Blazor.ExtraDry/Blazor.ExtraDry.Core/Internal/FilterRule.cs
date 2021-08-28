#nullable enable

using System.Collections.Generic;

namespace Blazor.ExtraDry.Core.Internal {

    internal class FilterRule {

        public FilterRule(string propertyName, FilterRule values)
        {
            PropertyName = propertyName;
            LowerBound = values.LowerBound;
            UpperBound = values.UpperBound;
            Values.AddRange(values.Values);
        }

        public FilterRule(string propertyName, BoundRule lowerBound, string lowerValue, BoundRule upperBound, string upperValue)
        {
            PropertyName = propertyName;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Values.Add(lowerValue);
            Values.Add(upperValue);
        }

        public FilterRule(string propertyName, string value)
        {
            PropertyName = propertyName;
            Values.Add(value);
        }

        public FilterRule(string propertyName, IEnumerable<string> values)
        {
            PropertyName = propertyName;
            Values = new List<string>(values);
        }

        public BoundRule LowerBound { get; private set; } = BoundRule.None;

        public BoundRule UpperBound { get; private set; } = BoundRule.None;

        public string PropertyName { get; private set; }

        public List<string> Values { get; private set; } = new List<string>();
    }

}
