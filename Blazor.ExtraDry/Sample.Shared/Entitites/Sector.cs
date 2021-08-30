using Blazor.ExtraDry;
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
        [Rules(UpdateAction.Ignore)]
        [JsonIgnore]
        public int Id { get; set; }

        [Rules(UpdateAction.Ignore)]
        public Guid Uuid { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        [Display(Name = "Title", ShortName = "Title")]
        public string Title { get; set; }

        [MaxLength(250)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        
        [Rules(DeleteValue = SectorState.Inactive)]
        public SectorState State { get; set; }

        public override string ToString() => Title;

        public override bool Equals(object obj) => (obj as Sector)?.Uuid == Uuid;

        public override int GetHashCode() => Uuid.GetHashCode();
        

    }
}
