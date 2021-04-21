using System;
using System.ComponentModel.DataAnnotations;

namespace Blazor.ExtraDry.Sample.Shared
{
    public class Employee
    {
        [Rules(UpdateAction.BlockChanges)]
        [Key]
        public int Id { get; set; }

        public Guid UniqueId { get; set; }

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
