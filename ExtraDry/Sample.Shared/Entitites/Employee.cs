#nullable enable

using ExtraDry.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sample.Shared
{
    public class Employee
    {
        [Key]
        [Rules(RuleAction.Block)]
        [JsonIgnore]
        public int Id { get; set; }

        public Guid Uuid { get; set; } = Guid.NewGuid();

        [Required, MaxLength(50)]
        [Rules(RuleAction.Allow)]
        [Display(Name = "First Name", ShortName = "First Name")]
        [Filter(FilterType.Equals)]
        public string? FirstName { get; set; }

        [Required, MaxLength(50)]
        [Rules(RuleAction.Allow)]
        [Display(Name = "Last Name", ShortName = "Last Name")]
        [Filter(FilterType.StartsWith)]
        public string? LastName { get; set; }

        /// <summary>
        /// The version info which informs the audit log.
        /// </summary>
        [JsonIgnore]
        public VersionInfo Version { get; set; } = new VersionInfo();

    }
}
