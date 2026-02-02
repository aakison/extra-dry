namespace ExtraDry.Blazor;

public interface IDryForm
{
    public object UntypedModel { get; }

    public string ModelNameSlug { get; }

    public DecoratorInfo? Description { get; set; }

    public FormDescription? FormDescription { get; set; }

}
