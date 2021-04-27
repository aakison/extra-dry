using Blazor.ExtraDry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sample.Shared {
    public class Company {

        [Key]
        [Rules(UpdateAction.BlockChanges)]
        public int Id { get; set; }

        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [Display(Name = "Name", ShortName = "Name")]
        public string Name { get; set; }

        [Display(Name = "Banking Details")]
        [Rules(UpdateAction.AllowChanges, CreateAction = CreateAction.CreateNew)]
        public BankingDetails BankingDetails { get; set; } = new BankingDetails();

        [Display(Name = "Videos")]
        [Rules(UpdateAction.AllowChanges, CreateAction = CreateAction.CreateNew)]
        public ICollection<Video> Videos { get; set; } = new Collection<Video>();

    }
}
