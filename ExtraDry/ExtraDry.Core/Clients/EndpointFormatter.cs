namespace ExtraDry.Core;

internal class EndpointFormatter
{
    public Type KeyType { get; set; } = typeof(object);

    public string ParmeterName { get; set; } = "";

    public Func<object, string> Formatter { get; set; } = e => e.ToString() ?? "";

    public EndpointMode Mode { get; set; } = EndpointMode.Append;

    public CrudOperation Operations { get; set; } = CrudOperation.All;
}

