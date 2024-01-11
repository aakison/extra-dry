﻿using System.Diagnostics.CodeAnalysis;

namespace Sample.Data.Services;

public class InMemoryBlobService
{
    public InMemoryBlobService(RuleEngine ruleEngine, FileValidator fileValidator)
    {
        rules = ruleEngine;
        validator = fileValidator;
    }

    public async Task<Blob> CreateAsync(Blob item)
    {
        Validate(item);
        memoryBlobStore.Add(item.Uuid, item);
        return await Task.FromResult(item);
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Could move if/when fake database replaced with file system.")]
    public async Task<Blob?> TryRetrieveAsync(Guid uuid)
    {
        if(memoryBlobStore.TryGetValue(uuid, out Blob? value)) {
            return await Task.FromResult(value);
        }
        else {
            return null;
        }
    }

    public async Task<Blob> RetrieveAsync(Guid uuid)
    {
        return await TryRetrieveAsync(uuid) ?? throw new ArgumentOutOfRangeException(nameof(uuid));
    }

    public async Task UpdateAsync(Blob item)
    {
        Validate(item);
        var existing = await RetrieveAsync(item.Uuid);
        await rules.UpdateAsync(item, existing);
        memoryBlobStore[item.Uuid] = existing;
    }

    public async Task DeleteAsync(Guid uuid)
    {
        var existing = await RetrieveAsync(uuid);
        await rules.DeleteAsync(existing, () => memoryBlobStore.Remove(uuid), () => { });
    }

    private void Validate(Blob item)
    {
        var dataValidator = new DataValidator();
        dataValidator.ValidateObject(item);
        dataValidator.ThrowIfInvalid();

        validator.ValidateFile(item);
        validator.ThrowIfInvalid();
    }

    private static readonly Dictionary<Guid, Blob> memoryBlobStore = new();

    private readonly RuleEngine rules;

    private readonly FileValidator validator;
}