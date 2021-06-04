using Blazor.ExtraDry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sample.Shared {
    public class Company {

        [Key]
        [Rules(UpdateAction.IgnoreChanges)]
        public int Id { get; set; }

        [Rules(UpdateAction.IgnoreChanges)]
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [Display(Name = "Name", ShortName = "Name")]
        [Filter]
        public string Name { get; set; }

        [Display]
        [MaxLength(1000)]
        [Rules(UpdateAction.IgnoreDefaults)]
        public string Description { get; set; }

        [Display]
        [Rules(CreateAction = CreateAction.LinkExisting)]
        public Sector PrimaryService { get; set; }

        // Attempt to reproduce viewmodel problem.
        //[Display(Name = "Alternate")]
        //public string Name2 {
        //    get => Name;
        //    set => Name = value;
        //}

        [Display]
        [Rules(UpdateAction.AllowChanges, CreateAction = CreateAction.CreateNew)]
        public BankingDetails BankingDetails { get; set; } = new BankingDetails();

        [Display]
        [Rules(UpdateAction.AllowChanges, CreateAction = CreateAction.CreateNew)]
        public ICollection<Video> Videos { get; set; } = new Collection<Video>();

    }
}
