#nullable enable

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

        public Guid Uuid { get; set; } = Guid.NewGuid();

        [Required, MaxLength(50)]
        [Rules(UpdateAction.AllowChanges)]
        [Display(Name = "First Name", ShortName = "First Name")]
        [Filter(FilterType.Equals)]
        public string? FirstName { get; set; }

        [Required, MaxLength(50)]
        [Rules(UpdateAction.AllowChanges)]
        [Display(Name = "Last Name", ShortName = "Last Name")]
        [Filter(FilterType.StartsWith)]
        public string? LastName { get; set; }
    }
}
