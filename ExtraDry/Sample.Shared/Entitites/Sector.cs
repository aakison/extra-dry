using ExtraDry.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sample.Shared {

    /// <summary>
    /// Represents a service that a company may provide.
    /// This is for properties that may appear as Enums, but have additional data associated with them.
    /// </summary>
    public class Sector {

        /// <summary>
        /// A locally unique identifier, internal use only.
        /// </summary>
        [Key]
        [Rules(RuleAction.Ignore)]
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// The globally unique identifier for this sector.
        /// </summary>
        [Rules(RuleAction.Ignore)]
        public Guid Uuid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The title of the sector
        /// </summary>
        /// <example>Commerical Electrical Services</example>
        [Required]
        [MaxLength(50)]
        [Display(Name = "Title", ShortName = "Title")]
        [Filter(FilterType.Contains)]
        public string Title { get; set; }

        /// <summary>
        /// The description of the sector.
        /// </summary>
        /// <example>Provides licensed electrical works for commercial facilities.</example>
        [MaxLength(250)]
        [Display(Name = "Description")]
        [Filter(FilterType.Contains)]
        public string Description { get; set; }
        
        /// <summary>
        /// The current status of the sector.
        /// </summary>
        [Rules(DeleteValue = SectorState.Inactive)]
        [Filter(FilterType.Equals)]
        public SectorState State { get; set;  }

        /// <summary>
        /// Display title for the sector.
        /// </summary>
        public override string ToString() => Title;

        /// <summary>
        /// Entity equality comparer, as uniquely defined by the `Uuid`.
        /// </summary>
        public override bool Equals(object obj) => (obj as Sector)?.Uuid == Uuid;

        /// <summary>
        /// Entity hash code, as uniquely defined by the `Uuid`.
        /// </summary>
        public override int GetHashCode() => Uuid.GetHashCode();

    }
}
