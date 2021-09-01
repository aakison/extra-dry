using Blazor.ExtraDry;
using Sample.Shared.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        [Rules(CreateAction = CreateAction.LinkExisting)]
        [JsonConverter(typeof(SectorInfoJsonConverter))]
        public Sector PrimarySector { get; set; }

        [Display]
        [Rules(CreateAction = CreateAction.LinkExisting)]
        public List<Sector> AdditionalSectors { get; set; }

        [Display]
        [Rules(RuleAction.Allow, CreateAction = CreateAction.CreateNew)]
        public BankingDetails BankingDetails { get; set; } = new BankingDetails();

        [Display]
        [Rules(RuleAction.Allow, CreateAction = CreateAction.CreateNew)]
        public ICollection<Video> Videos { get; set; } = new Collection<Video>();

    }
}
