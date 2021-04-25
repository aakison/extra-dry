using Blazor.ExtraDry;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sample.Shared
{
    public class Employee
    {
        [Key]
        [Rules(UpdateAction.BlockChanges)]
        public int Id { get; set; }

        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [Required, MaxLength(50)]
        [Rules(UpdateAction.AllowChanges)]
        [Display(Name = "First Name", ShortName = "First Name")]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        [Rules(UpdateAction.AllowChanges)]
        [Display(Name = "Last Name", ShortName = "Last Name")]
        public string LastName { get; set; }
    }
}
