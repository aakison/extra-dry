using ExtraDry.Core;
using ExtrayDry.Core;
using Sample.Shared.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sample.Shared {

    public class Company {

        [Key]
        [JsonIgnore]
        [Rules(RuleAction.Ignore)]
        public int Id { get; set; }

        [Rules(RuleAction.Ignore)]
        public Guid Uuid { get; set; } = Guid.NewGuid();

        [Display(Name = "Name", ShortName = "Name")]
        [Filter]
        public string Name { get; set; }

        [Display]
        [MaxLength(1000)]
        [Rules(RuleAction.IgnoreDefaults)]
        public string Description { get; set; }

        [Display]
        [Rules(RuleAction.Link)]
        [JsonConverter(typeof(SectorInfoJsonConverter))]
        public Sector PrimarySector { get; set; }

        [Display]
        [Rules(RuleAction.Link)]
        public List<Sector> AdditionalSectors { get; set; }

        [Display]
        [Rules(RuleAction.Allow)]
        public BankingDetails BankingDetails { get; set; } = new BankingDetails();

        //[Display]
        //[Rules(RuleAction.Recurse)]
        //public ICollection<Video> Videos { get; set; } = new Collection<Video>();

        /// <summary>
        /// The version info which informs the audit log.
        /// </summary>
        [JsonIgnore]
        public VersionInfo Version { get; set; } = new VersionInfo();

    }
}
