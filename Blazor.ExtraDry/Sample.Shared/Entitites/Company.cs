using Blazor.ExtraDry;
using System;
using System.Collections.Generic;
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

        [Rules(UpdateAction.AllowChanges)]
        [Display(Name = "Social Media")]
        public SocialMedia SocialMedia { get; set; } = new SocialMedia();

    }
}
