﻿using Blazor.ExtraDry;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sample.Shared {

    /// <summary>
    /// Represents a service that a company may provide.
    /// This is for properties that may appear as Enums, but have additional data associated with them.
    /// </summary>
    public class Sector {

        [Key]
        [Rules(UpdateAction.IgnoreChanges)]
        [JsonIgnore]
        public int Id { get; set; }

        [Rules(UpdateAction.IgnoreChanges)]
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        [Display(Name = "Title", ShortName = "Title")]
        public string Title { get; set; }

        [MaxLength(250)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Rules(DeleteValue = SectorState.Inactive)]
        public SectorState State { get; set; }

        public override string ToString() => Title;

        public override bool Equals(object obj) => (obj as Sector)?.UniqueId == UniqueId;

        public override int GetHashCode() => UniqueId.GetHashCode();
        

    }
}