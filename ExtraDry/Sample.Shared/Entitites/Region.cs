namespace Sample.Shared;

/// <summary>
/// Represents a single geo-political region in a taxonomy of geo-political regions.
/// </summary>
public class Region : TaxonomyEntity<Region>, ITaxonomyEntity, INamedSubject, IValidatableObject {

    /// <summary>
    /// The principal ID for the region, internal to the database.
    /// </summary>
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// The level for this region inside a taxonomy of regions.
    /// </summary>
    public RegionLevel Level { get; set; }

    /// <summary>
    /// The strata for the entity in the taxonomy, 0 is root, each level adds 1.
    /// </summary>
    [JsonIgnore]
    public int Strata => (int)Level;

    /// <summary>
    /// The code for the region, using the ISO-3166 standard.
    /// Use alpha-2 codes for country, then country specific codes.  E.g. "AU", then "AU-QLD", then "AU-QLD-Brisbane".
    /// </summary>
    [Required, StringLength(32)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// The short name of the country or region, such as 'Australia', or 'USA'.
    /// </summary>
    [Required, StringLength(32)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The full name of the country or region, such as 'Commonwealth of Australia', or 'United States of America'.
    /// </summary>
    /// <remarks>
    /// Limited to 100 characters based on full names of countries which, in English, max at 59 characters per ISO.
    /// </remarks>
    [Required, StringLength(100)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Validates the code is in the right ISO-3166 format based on the level of the region.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        var codeRegex = Level switch {
            RegionLevel.Country => CountryRegex,
            RegionLevel.Division => DivisionRegex,
            _ => SubdivisionRegex,
        };
        if(!codeRegex.IsMatch(Code)) {
            results.Add(new ValidationResult("Code must follow ISO-3166 naming scheme, e.g. 'AU', 'AU-QLD', 'AU-QLD-Brisbane'."));
        }
        return results;
    }

    private static readonly Regex CountryRegex = new Regex(@"\w{2}", RegexOptions.Compiled);
    private static readonly Regex DivisionRegex = new Regex(@"\w{2}-\w{2,4}", RegexOptions.Compiled);
    private static readonly Regex SubdivisionRegex = new Regex(@"\w{2}-\w{2,4}-\w{2,20}", RegexOptions.Compiled);

}
