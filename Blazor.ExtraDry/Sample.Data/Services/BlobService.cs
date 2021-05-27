#nullable enable

using Blazor.ExtraDry;
using Microsoft.EntityFrameworkCore;
using Sample.Shared;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task Create(BlobInfo item, Stream contentStream, int length)
        {
            if(await database.Blobs.AnyAsync(e => e.Name == item.Name && e.Scope == item.Scope)) {
                throw new ArgumentException("Blob already exists", nameof(item));
            }
            database.Blobs.Add(item);
            await database.SaveChangesAsync();
            Upload(item, contentStream, length);
        }

        public async Task<BlobInfo> Retrieve(Guid uniqueId)
        {
            return await database.Blobs.FirstOrDefaultAsync(e => e.UniqueId == uniqueId);
        }

        public async Task Update(BlobInfo item)
        {
            var existing = await Retrieve(item.UniqueId);
            rules.Update(item, existing);
            await database.SaveChangesAsync();
        }

        public async Task UploadAsync(Guid uniqueId, Stream stream, int length)
        {
            var existing = await Retrieve(uniqueId);
            if(existing == default) {
                throw new ArgumentOutOfRangeException("UniqueId for blob not found", nameof(uniqueId));
            }
            Upload(existing, stream, length);
        }

        public void Upload(BlobInfo item, Stream stream, int length)
        {
            using var reader = new BinaryReader(stream);
            var bytes = reader.ReadBytes(length);
            fakeBlobStorage.Add(FakeKey(item), bytes);
        }

        public async Task Delete(Guid uniqueId)
        {
            var existing = await Retrieve(uniqueId);
            rules.Delete(existing, () => database.Blobs.Remove(existing));
            await database.SaveChangesAsync();
        }

        private static string FakeKey(BlobInfo info) => $"{info.Scope}/{info.Name}";

        private readonly SampleContext database;

        private readonly RuleEngine rules;

        private readonly Dictionary<string, byte[]> fakeBlobStorage = new();

    }
}
