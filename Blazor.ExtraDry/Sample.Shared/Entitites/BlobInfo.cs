#nullable enable

using Blazor.ExtraDry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Sample.Shared {
    public class BlobInfo {

        [Key]
        [Rules(UpdateAction.BlockChanges)]
        [JsonIgnore]
        public int Id { get; set; }

        [Rules(UpdateAction.BlockChanges)]
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [Display]
        [Filter]
        public string Scope { get; set; } = "public";

        [Display(Name = "Name", ShortName = "Name")]
        [Filter]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The Url for the blob that allows direct access without using this API.
        /// </summary>
        public string Url { get; set; } = string.Empty;

    }
}
