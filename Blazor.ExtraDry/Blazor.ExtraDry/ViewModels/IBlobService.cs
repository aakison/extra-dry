using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.ExtraDry.ViewModels {

    public interface IBlob {

        public Guid UniqueId { get; set; }

        public string Filename { get; set; }

        public string Uri { get; set; }
    }

    public interface IBlobStorageService<T> where T : IBlob {
        Task<ICollection<T>> ListAsync(string prefix, Dictionary<string, object> metaDataFilter = null);
        Task<T> CreateAsync(byte[] content, string mimeType = null, string filename = null, Dictionary<string, object> metaData = null);
        Task<T> RetrieveAsync(string filename);
        Task<T> UpdateAsync(T exemplar);
        Task DeleteAsync(string filename);

        Task<Stream> OpenBlob(T blob);
        Task<Stream> OpenBlob(string filename);
    }

}
