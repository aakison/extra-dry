using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Shared;

public class Employee : IResourceIdentifiers, ICreatingCallback
{
    [Key]
    [Rules(RuleAction.Block)]
    [JsonIgnore]
    public int Id { get; set; }

    [Rules(RuleAction.Block)]
    public Guid Uuid { get; set; } = Guid.NewGuid();

    [Required, StringLength(50)]
    public string Slug { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [Display(Name = "Full Name", ShortName = "Name")]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(50)]
    [Rules(RuleAction.Allow)]
    [Display(Name = "First Name", ShortName = "First Name")]
    [Filter(FilterType.Equals)]
    public string? FirstName { get; set; }

    [Required, StringLength(50)]
    [Rules(RuleAction.Allow)]
    [Display(Name = "Last Name", ShortName = "Last Name")]
    [Filter(FilterType.StartsWith)]
    public string? LastName { get; set; }

    [StringLength(120)]
    [EmailAddress]
    [Display(Name = "Email Address", ShortName = "Email")]
    [Filter(FilterType.Contains)]
    public string? Email { get; set; }

    /// <summary>
    /// The last date the employee worked for the company.
    /// </summary>
    /// <remarks>Use for testing nullable dates in Filter conditions.</remarks>
    [Filter]
    public DateTime? TerminationDate { get; set; }

    [NotMapped]
    [Display(ShortName = "Active")]
    public bool ActiveEmployee => TerminationDate == null || TerminationDate > DateTime.Now;

    [JsonConverter(typeof(ResourceReferenceConverter<Company>))]
    public Company? Employer { get; set; }

    /// <summary>
    /// The version info which informs the audit log.
    /// </summary>
    [JsonIgnore]
    public VersionInfo Version { get; set; } = new VersionInfo();

    public override string ToString() => $"{FirstName} {LastName}";

    public Task OnCreatingAsync()
    {
        if(string.IsNullOrEmpty(Slug)) {
            Slug = Uuid.ToString().ToLowerInvariant();
        }
        if(string.IsNullOrEmpty(Title)) {
            Title = $"{FirstName} {LastName}";
        }
        return Task.CompletedTask;
    }

}
