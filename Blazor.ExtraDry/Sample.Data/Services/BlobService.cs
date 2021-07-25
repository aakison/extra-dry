#nullable enable

using Blazor.ExtraDry;
using Microsoft.EntityFrameworkCore;
using Sample.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Sample.Data.Services {
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
            using var sha256Hash = SHA256.Create();
            var hash = sha256Hash.ComputeHash(content);
            var hashString = string.Join("", hash.Select(e => e.ToString("X2")));

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
            item.UniqueId = Guid.NewGuid();
            item.Url = $"/api/blobs/{item.UniqueId}/content";

            database.Blobs.Add(item);
            await database.SaveChangesAsync();
            fakeBlobStorage.Add(item.UniqueId, content);
            return item;
        }

        public async Task<byte[]> DownloadAsync(BlobInfo item)
        {
            if(fakeBlobStorage.TryGetValue(item.UniqueId, out var content)) {
                return content;
            }
            else {
                await Task.Delay(1);
                throw new ArgumentException("Can't find content for item");
            }
        }

        public async Task<BlobInfo> RetrieveAsync(Guid uniqueId)
        {
            return await database.Blobs.FirstOrDefaultAsync(e => e.UniqueId == uniqueId);
        }

        public async Task UpdateAsync(BlobInfo item)
        {
            var existing = await RetrieveAsync(item.UniqueId);
            await rules.UpdateAsync(item, existing);
            await database.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid uniqueId)
        {
            var existing = await RetrieveAsync(uniqueId);
            rules.Delete(existing, () => database.Blobs.Remove(existing));
            await database.SaveChangesAsync();
        }

        private readonly SampleContext database;

        private readonly RuleEngine rules;

        private static readonly Dictionary<Guid, byte[]> fakeBlobStorage = new();

    }
}
