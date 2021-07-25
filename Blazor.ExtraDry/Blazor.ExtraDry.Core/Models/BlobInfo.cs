#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Blazor.ExtraDry {
    public class BlobInfo : IBlobInfo {

        [Key]
        [Rules(UpdateAction.BlockChanges)]
        [JsonIgnore]
        public int Id { get; set; }

        [Rules(UpdateAction.BlockChanges)]
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [Display]
        [Filter]
        public BlobScope Scope { get; set; }

        public int Size { get; set; }

        [MaxLength(64)]
        public string ShaHash { get; set; } = string.Empty;

        /// <summary>
        /// The Url for the blob that allows direct access without using this API.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        [Display(Name = "Name", ShortName = "Name")]
        [Filter]
        public string Filename { get; set; } = string.Empty;

        /// <summary>
        /// The mime type of the blob which is delivered along with the blob when requested.
        /// Defaults to `application/octet-string` which is the most generic option.
        /// </summary>
        public string MimeType { get; set; } = "application/octet-string";
    }
}
