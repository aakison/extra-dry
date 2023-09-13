namespace ExtraDry.Core;

public abstract class Constraint
{
    public string Title { get; set; } = string.Empty;

    public bool Enabled { get; set; } = true;

    public abstract bool IsValid(object value);
}

public class StringLengthConstraint : Constraint {

    public StringLengthConstraint()
    {
        Title = "Text Length";
    }

    public override bool IsValid(object value)
    {
        throw new NotImplementedException();
    }

    public int MaximumCharacters { get; set; } = 100;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ConstraintDataTypeAttribute : Attribute
{
    public ConstraintDataTypeAttribute(ExpandoDataType type) {
        DataType = type;
    }

    public ExpandoDataType DataType { get; private set; }
}

[ConstraintDataType(ExpandoDataType.Text)]
public class ValidValuesConstraint : Constraint {

    /// <summary>
    /// A list of values that constrain the field.  If empty, no valid value constaint is applied.
    /// </summary>
    public List<ValidValue> ValidValues { get; set; } = new();

    public override bool IsValid(object value)
    {
        throw new NotImplementedException();
    }
}

public class RequiredConstraint : Constraint {
    public RequiredConstraint()
    {
        Title = "Required";
    }

    public override bool IsValid(object value)
    {
        return !string.IsNullOrWhiteSpace(value?.ToString());
    }
}

[ConstraintDataType(ExpandoDataType.Number)]
[ConstraintDataType(ExpandoDataType.Currency)]
public class NumericRangeConstraint : Constraint {
    public override bool IsValid(object value)
    {
        var number = Convert.ToDouble(value);
        return number >= Minimum && number <= Maximum;
    }

    public double Minimum { get; set; } = double.MinValue;

    public double Maximum { get; set; } = double.MaxValue;
}

[ConstraintDataType(ExpandoDataType.DateTime)]
public class DateRangeConstraint : Constraint {

    public override bool IsValid(object value)
    {
        var date = Convert.ToDateTime(value);
        return date >= Minimum && date <= Maximum;
    }

    public DateTime Minimum { get; set; } = DateTime.MinValue;

    public DateTime Maximum { get; set; } = DateTime.MaxValue;
}

