using Blazor.ExtraDry;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sample.Shared {

    /// <summary>
    /// Represents a service that a company may provide.
    /// This is for properties that may appear as Enums, but have additional data associated with them.
    /// </summary>
    public class Service {

        [Key]
        [Rules(UpdateAction.BlockChanges)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }

        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        [Display(Name = "Title", ShortName = "Title")]
        public string Title { get; set; }

        [MaxLength(100)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        
        public ServiceState State { get; set; }
    }
}
