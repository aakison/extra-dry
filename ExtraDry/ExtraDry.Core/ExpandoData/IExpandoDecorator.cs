namespace ExtraDry.Core.ExpandoData;

public interface IExpandoDecorator
{
    public Task<ExpandoSchema> GetSchemaAsync();
}
