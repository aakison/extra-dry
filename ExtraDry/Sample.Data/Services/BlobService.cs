using System.Diagnostics.CodeAnalysis;

namespace Sample.Data.Services;

public class BlobService {

    public BlobService(SampleContext sampleContext, RuleEngine ruleEngine, FileValidator fileValidator)
    {
        database = sampleContext;
        rules = ruleEngine;
        validator = fileValidator;
    }

    public async Task<Blob> CreateAsync(Blob item)
    {
        var dataValidator = new DataValidator();
        dataValidator.ValidateObject(item);
        dataValidator.ThrowIfInvalid();

        validator.ValidateFile(item);
        validator.ThrowIfInvalid();

        fakeBlobStorage.Add(item.Uuid, item);
        return await Task.FromResult(item);
    }

    public async Task<Blob> RetrieveAsync(Guid uuid)
    {
        return await TryRetrieveAsync(uuid) ?? throw new ArgumentOutOfRangeException(nameof(uuid));
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Could move if/when fake database replaced with file system.")]
    public async Task<Blob?> TryRetrieveAsync(Guid uuid)
    {
        if(fakeBlobStorage.TryGetValue(uuid, out Blob? value)) {
            return await Task.FromResult(value);
        }
        else {
            return null;
        }
    }

    public async Task UpdateAsync(Blob item)
    {
        var existing = await RetrieveAsync(item.Uuid);
        await rules.UpdateAsync(item, existing);
        await database.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid uuid)
    {
        var existing = await RetrieveAsync(uuid);
        await rules.DeleteAsync(existing, () => fakeBlobStorage.Remove(uuid), () => { });
    }

    private readonly SampleContext database;

    private readonly RuleEngine rules;

    private static readonly Dictionary<Guid, Blob> fakeBlobStorage = new();

    private readonly FileValidator validator;
}

