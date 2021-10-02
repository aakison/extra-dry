#nullable enable

using System.ComponentModel.DataAnnotations;

namespace ExtraDry.Core {

    /// <summary>
    /// Validates a decimal value as being within the given range.
    /// Functionally the same as the RangeAttribute but works for decimals.
    /// </summary>
    public class DecimalRangeAttribute : ValidationAttribute {

        /// <summary>
        /// The minimum, inclusive, lower bound for the value.
        /// </summary>
        public decimal Minimum { get; set; }

        /// <summary>
        /// The maximum, inclusive, upper bound for the value.
        /// </summary>
        public decimal Maximum { get; set; }

        /// <summary>
        /// Create a new validator for a decimal range
        /// </summary>
        /// <remarks>
        /// Constructor uses double since attibutes need CLR types.
        /// </remarks>
        public DecimalRangeAttribute(double minimum, double maximum)
        {
            Minimum = (decimal)minimum;
            Maximum = (decimal)maximum;
        }

        /// <summary>
        /// Standard validation override.
        /// </summary>
        public override bool IsValid(object? value) => 
            value switch {
                null => true,
                decimal decimalValue => Minimum <= decimalValue && decimalValue <= Maximum,
                _ => false,
            };

    }
}
