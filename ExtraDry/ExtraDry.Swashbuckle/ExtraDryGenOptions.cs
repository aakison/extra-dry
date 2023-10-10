using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Swashbuckle;

public class ExtraDryGenOptions
{
    /// <summary>
    /// Options for the display of the instructions page as a seperate OpenAPI document.
    /// </summary>
    public InstructionPageOptions Instructions { get; } = new();

    /// <summary>
    /// Options for the inclusion of XML Documentation files.  Some features require that XML 
    /// documentation is added in a particular order so this is recommended over using 
    /// IncludeXmlComments directly.
    /// </summary>
    public DocumentationXmlOptions XmlComments { get; } = new();

    /// <summary>
    /// Options for turning on/off specific filters that augment the OpenAPI document.
    /// </summary>
    public FilterOptions Filters { get; } = new();

    public class InstructionPageOptions {

        public bool Include { get; set; } = true;

        [SuppressMessage("Security", "DRY1304:Properties that might leak PID should be JsonIgnore.", Justification = "Just a version Id")]
        public string Version { get; set; } = "v1";

        public string Title { get; set; } = "API Instructions";

        public string Description { get; set; } = "This API provides consistent access to services available on this system conforming to Extra DRY principles.  The following instructions are applied consistently across the entire API set.";
    }

    /// <summary>
    /// Options that control the inclusion of XML documentation.
    /// </summary>
    public class DocumentationXmlOptions {

        /// <summary>
        /// Indicates if the documentation for Extra Dry classes is included (recommended).
        /// </summary>
        public bool IncludeExtraDryDocuments { get; set; } = true;

        /// <summary>
        /// The names of the XML document files to add (without paths)
        /// </summary>
        public List<string> Files { get; set; } = new();

    }

    public class FilterOptions
    {
        /// <summary>
        /// The signature of endpoints will generate status codes.  E.g. Authorize attribute 
        /// implies status code of 503 Forbidden is valid.
        /// </summary>
        public bool EnableSignatureStatusCodes { get; set; } = true;

        /// <summary>
        /// Endpoints that take Sort, Filter or Page Queries will have documentation added for
        /// users describing the valid properties for each endpoints query.
        /// </summary>
        public bool EnableQueryDocumentation { get; set; } = true;  

        /// <summary>
        /// Properties in schema objects that can be inferred to be read-only are set as read-only 
        /// in the OpenAPI document.
        /// </summary>
        public bool EnableReadOnlyOnSchema { get; set; } = true;

        /// <summary>
        /// Objects in schema that have Json Serializer that casts the object to a Resource 
        /// Reference will be exposed as the Resource Reference in the OpenAPI schema instead of 
        /// using the full object representation.
        /// </summary>
        public bool EnableResourceReferenceSchemaRewrite { get; set; } = true;

        /// <summary>
        /// API Controllers with a Display attribute will use the name and order from the display
        /// attribute to control the name and order of Tags in the OpenAPI document.  These tags
        /// are used to display names and groups of endpoints in the Swagger UI.
        /// </summary>
        public bool EnableDisplayOnApiControllers { get; set; } = true;

    }
}

