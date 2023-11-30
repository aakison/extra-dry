#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Sample.Data.Services;

public class BlobService {

    public BlobService(SampleContext sampleContext, RuleEngine ruleEngine)
    {
        database = sampleContext;
        rules = ruleEngine;
    }

    public async Task<PagedCollection<BlobInfo>> List(PageQuery query)
    {
        return await database.Blobs.QueryWith(query).ToPagedCollectionAsync();
    }

    public async Task<BlobInfo> UploadAsync(BlobInfo item, byte[] content)
    {
        var hash = SHA256.HashData(content);
        var hashString = string.Join("", hash.Select(e => e.ToString("X2", CultureInfo.InvariantCulture)));

        var existing = await database.Blobs.FirstOrDefaultAsync(e => e.ShaHash == hashString && e.Scope == BlobScope.Public);
        if(existing != null) {
            return existing;
        }

        if(content.Length > 4 && content[1] == 'P' && content[2] == 'N' && content[3] == 'G') {
            item.MimeType = "image/png";
        }
        if(content.Length > 4 && content[1] == 'P' && content[2] == 'D' && content[3] == 'F') {
            item.MimeType = "application/pdf";
        }
        if(content.Length > 2 && content[0] == 0xFF && content[1] == 0xD8) {
            // TODO: Is it necessary to check content for "JFIF" or "Exif" as well?
            item.MimeType = "image/jpeg";
        }
        item.ShaHash = hashString;
        item.Uuid = Guid.NewGuid();
        item.Url = $"/api/blobs/{item.Uuid}/content";

        database.Blobs.Add(item);
        await database.SaveChangesAsync();
        fakeBlobStorage.Add(item.Uuid, content);
        return item;
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "When not faked, will require instance.")]
    public async Task<byte[]> DownloadAsync(BlobInfo item)
    {
        if(fakeBlobStorage.TryGetValue(item.Uuid, out var content)) {
            return content;
        }
        else {
            await Task.Delay(1);
            throw new ArgumentException("Can't find content for item");
        }
    }

    public async Task<BlobInfo> RetrieveAsync(Guid uniqueId)
    {
        var result = await TryRetrieveAsync(uniqueId) ?? throw new ArgumentOutOfRangeException(nameof(uniqueId));
        return result;
    }

    public async Task<BlobInfo?> TryRetrieveAsync(Guid uniqueId)
    {
        return await database.Blobs.FirstOrDefaultAsync(e => e.Uuid == uniqueId);
    }

    public async Task UpdateAsync(BlobInfo item)
    {
        var existing = await RetrieveAsync(item.Uuid);
        await rules.UpdateAsync(item, existing);
        await database.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid uniqueId)
    {
        var existing = await RetrieveAsync(uniqueId);
        await rules.DeleteAsync(existing, () => database.Blobs.Remove(existing), async () => await database.SaveChangesAsync());
    }

    private readonly SampleContext database;

    private readonly RuleEngine rules;

    private static readonly Dictionary<Guid, byte[]> fakeBlobStorage = new();

}

