#nullable enable

using System;
using System.ComponentModel.DataAnnotations;

namespace Blazor.ExtraDry {

    public interface IBlobInfo {

        public Guid UniqueId { get; set; }

        public string Filename { get; set; }

        public string Url { get; set; }

        [MaxLength(256)]
        public string MimeType { get; set; }
    }

    public enum BlobScope {
        Private,
        Public,
    }
}
