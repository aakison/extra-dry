using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtraDry.Core;

/// <summary>
/// Core service which provides functionality for validating files against a set of validation
/// rules.  The validation is partially performed client-side and a complete validation is
/// performed server-side.  Configuration of the file validation rules are done using the
/// <see cref="ServiceCollectionExtensions.AddFileValidation"/> extension method during startup.
/// </summary>
public class FileValidationService : IFileValidationOptions
{

    /// <summary>
    /// Configures the file validation service.  Not recommended to use directly, instead use the
    /// <see cref="ServiceCollectionExtensions.AddFileValidation"/> extension method during startup.
    /// </summary>
    public FileValidationService(FileValidationOptions options)
    {
        if(options.OptionsFilepath != null) {
            fileService = new FileTypeDefinitionSource(options.OptionsFilepath);
        }
        else {
            fileService = new FileTypeDefinitionSource("");
        }
        extensionWhitelist = new List<string>(options.ExtensionWhitelist);
        extensionBlacklist = new List<BlacklistFileType>(options.ExtensionBlacklist);
        ConfigureUploadRestrictions(options);
        Options = options;
    }

    #region IFileValidationOptions interface

    /// <inheritdoc/>
    public ValidationCondition ValidateContent => Options.ValidateContent;

    /// <inheritdoc/>
    public ValidationCondition ValidateExtension => Options.ValidateExtension;

    /// <inheritdoc/>
    public ValidationCondition ValidateFilename => Options.ValidateFilename;

    /// <inheritdoc/>
    public FilenameCharacters FileCleanerAllowedExtensionCharacters => Options.FileCleanerAllowedExtensionCharacters;
    
    /// <inheritdoc/>
    public FilenameCharacters FileCleanerAllowedNameCharacters => Options.FileCleanerAllowedNameCharacters;

    /// <inheritdoc/>
    public bool FileCleanerCompressFilename => Options.FileCleanerCompressFilename;

    /// <inheritdoc/>
    public bool FileCleanerLowercase => Options.FileCleanerLowercase;

    /// <inheritdoc/>
    public ICollection<string> ExtensionWhitelist => new ReadOnlyCollection<string>(extensionWhitelist);
    private List<string> extensionWhitelist;

    /// <inheritdoc/>
    public ICollection<BlacklistFileType> ExtensionBlacklist => new ReadOnlyCollection<BlacklistFileType>(extensionBlacklist);
    private List<BlacklistFileType> extensionBlacklist;

    #endregion

    private void ConfigureUploadRestrictions(FileValidationOptions config)
    {
        if(config.ExtensionWhitelist.Count > 0) {
            extensionWhitelist = new(config.ExtensionWhitelist);
        }
        else {
            extensionWhitelist = new List<string> { "txt", "jpg", "png", "jpeg" };
        }

        FileTypeBlacklist.Clear();
        ExtensionOnlyBlacklist.Clear();

        if(config?.ExtensionBlacklist?.Count > 0) {
            FileTypeBlacklist.AddRange(config.ExtensionBlacklist.Where(f => f.CheckType == CheckType.BytesAndFilename).Select(f => f.Extension));
            ExtensionOnlyBlacklist.AddRange(config.ExtensionBlacklist.Where(f => f.CheckType == CheckType.FilenameOnly).Select(f => f.Extension));
            ExtensionOnlyBlacklist.AddRange(FileTypeBlacklist);
        }
        else {
            FileTypeBlacklist.AddRange(new List<string> {
                "asp", "aspx", "config", "ashx", "asmx", "aspq", "axd", "cshtm", "cshtml", "rem", "soap", "vbhtm", "vbhtml", "asa", "cer", "shtml", // Pentester Identified, classified under "ASP"
                "jsp", "jspx", "jsw", "jsv", "jspf", "wss", "do", "action", // Pentester Identified, classified under "JSP"
                "bat", "bin", "cmd", "com", "cpl", "exe", "gadget", "inf1", "ins", "inx", "isu", "job", "jse", "lnk", "msc", "msi", "msp", "mst", "paf", "pif", "ps1", "reg", "rgs", "scr", "sct", "shb", "shs", "u3p", "vb", "vbe", "vbs", "vbscript", "ws", "wsf", "wsh", // Pentester Identified, classified under "General"
                "py", "go", "app", "scpt", "scptd" // Additional filetypes.
            });
            ExtensionOnlyBlacklist.AddRange(FileTypeBlacklist);
            ExtensionOnlyBlacklist.AddRange(new List<string>() { "apk", "jar" });
        }
    }

    /// <summary>
    /// Clean a filename to ensure it is safe to use for web URIs and to transfer to other operating systems.
    /// </summary>
    public string CleanFilename(string filename)
    {
        // Sort out the filename part
        var fileOnly = Path.GetFileNameWithoutExtension(filename);

        // replace all the invalid characters
        var cleanedFilename = FileCleanerAllowedNameCharacters switch {
            FilenameCharacters.AsciiAlphaNumeric => Regex.Replace(fileOnly, InvalidAsciiCharacterRegex, "-"), 
            FilenameCharacters.UnicodeAlphaNumeric => Regex.Replace(fileOnly, InvalidUnicodeCharacterRegex, "-"),
            _ => fileOnly,
        };

        if(FileCleanerCompressFilename) {
            cleanedFilename = Regex.Replace(cleanedFilename, @"-+", "-"); // Collapse duplicate hyphens
            cleanedFilename = Regex.Replace(cleanedFilename, @"_+", "_"); // Collapse duplicate underscores
            cleanedFilename = Regex.Replace(cleanedFilename, @"[-\.]{2,}", "."); // Collapse -.- combinations to a .

            cleanedFilename = cleanedFilename.Trim('-');
            cleanedFilename = cleanedFilename.TrimEnd('_');
        }

        // Now the file extension, the only extensions that aren't alphanumeric are files that we likely should be rejecting
        // The addition of the hyphen as well allows for wordpress content and other similar proprietary but possible files.
        // https://www.file-extensions.org/extensions/begin-with-special-characters
        // https://www.file-extensions.org/filetype/extension/name/miscellaneous-files
        var extension = Path.GetExtension(filename).TrimStart('.');
        var cleanedExtension = FileCleanerAllowedExtensionCharacters switch {
            FilenameCharacters.AsciiAlphaNumeric => Regex.Replace(extension, InvalidAsciiCharacterRegex, ""),
            FilenameCharacters.UnicodeAlphaNumeric => Regex.Replace(extension, InvalidUnicodeCharacterRegex, ""),
            _ => extension,
        };

        // TODO: Clean up diacritics? https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net

        return $"{cleanedFilename}.{cleanedExtension}";
    }

    /// <summary>
    /// Determines whether a file can be uploaded, by using the following rules
    /// - The filename must not contain special characters
    /// - The filename must not have an extension that is in the blacklist
    /// - The filename must have an extension that is in the whitelist
    /// - The magic bytes, filename and mime type must match if present
    /// - If the file type is of a known xml type, it must not contain a script tag
    /// </summary>
    internal IEnumerable<ValidationResult> ValidateFile(string? filename, string? mimetype, byte[]? content = null)
    {
        // Still invalid but cleaner logic later.
        filename ??= ""; 
        mimetype ??= "";
        
        fileService ??= new();

        var extension = Path.GetExtension(filename).TrimStart('.');

        var filenameInferredTypes = fileService.GetFileTypeFromFilename(filename);
        var mimeInferredTypes = fileService.GetFileTypeFromMime(mimetype);

        // If there's both a filename and a mime type file definition, and they don't match, reject
        if(mimeInferredTypes.Any() &&
            filenameInferredTypes.Any() &&
            !filenameInferredTypes.SelectMany(f => f.MimeTypes).Intersect(mimeInferredTypes.SelectMany(f => f.MimeTypes)).Any()) { 
            yield return new ValidationResult($"Provided filename and mime type do not match, mime type was {GetFileDefinitionDescription(mimeInferredTypes)}, and filename was {GetFileDefinitionDescription(filenameInferredTypes)}");
        }

        var filenameErrors = ValidateFileFilename(filename).ToList();
        var extensionErrors = ValidateFileExtension(extension).ToList();
        var contentErrors = ValidateFileContent(content, filenameInferredTypes, extension).ToList();
        foreach(var error in contentErrors.Union(extensionErrors).Union(filenameErrors)) {
            yield return error;
        }
    }

    private IEnumerable<ValidationResult> ValidateFileFilename(string filename)
    {
        var validate = ValidateFilename switch {
            ValidationCondition.Never => false,
            ValidationCondition.Always => true,
            _ => !WebAssemblyRuntime,
        };
        if(!validate) {
            yield break;
        }

        if(string.IsNullOrEmpty(filename)) {
            yield return new ValidationResult("Provided filename was null or empty");
        }

        if(filename.Length > 255) {
            yield return new ValidationResult("Provided filename was too long");
        }

        // If the filename has bad characters in it and isn't already cleaned.
        if(filename != CleanFilename(filename)) {
            yield return new ValidationResult($"Provided filename contained invalid characters, '{filename}'");
        }
    }

    private IEnumerable<ValidationResult> ValidateFileExtension(string extension)
    {
        var validate = ValidateExtension switch {
            ValidationCondition.Never => false,
            ValidationCondition.Always => true,
            _ => !WebAssemblyRuntime,
        };
        if(!validate) {
            yield break;
        }

        if(string.IsNullOrEmpty(extension)) {
            yield return new ValidationResult($"Provided filename contains an invalid extension, '{extension}'");
        }

        // Outright reject executable file extensions - even if they're in the whitelist.
        if(ExtensionOnlyBlacklist.Contains(extension, StringComparer.OrdinalIgnoreCase)) {
            yield return new ValidationResult($"Provided filename belongs to a forbidden filetype, '{extension}'");
        }

        // If the extension isn't in the whitelist, reject it
        if(!extensionWhitelist.Contains(extension, StringComparer.OrdinalIgnoreCase)) {
            yield return new ValidationResult($"Provided filename does not belongs to an allowed filetype, '{extension}'");
        }

    }

    private IEnumerable<ValidationResult> ValidateFileContent(byte[]? content, IEnumerable<FileTypeDefinition> filenameInferredTypes, string extension)
    {
        var validate = ValidateContent switch {
            ValidationCondition.Never => false,
            ValidationCondition.Always => true,
            _ => !WebAssemblyRuntime,
        };

        if(!validate) {
            yield break;
        }
        if(content == null || content.Length == 0) {
            yield return new ValidationResult("File content is required", new[] { nameof(content) });
            yield break; // remainder of tests would throw exceptions...
        }
        var contentInferredTypes = fileService.GetFileTypeFromContent(content);

        // If the magic bytes filetype is in the bytes blacklist, reject
        var blacklistedType = contentInferredTypes
            .SelectMany(e => e.Extensions)
            .Intersect(FileTypeBlacklist, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault();
        if(blacklistedType != null) {
            yield return new ValidationResult($"Provided file content belongs to a forbidden filetype, {blacklistedType}", new[] { nameof(content) });
        }

        // If there's both a filename and a magic byte type file definition, and they don't match, reject
        if(contentInferredTypes.Any() && 
            filenameInferredTypes.Any() && 
            !filenameInferredTypes.SelectMany(f => f.MimeTypes).Intersect(contentInferredTypes.SelectMany(f => f.MimeTypes)).Any()) { 
            yield return new ValidationResult($"Provided filename and content do not match, {GetFileDefinitionDescription(contentInferredTypes)} vs {GetFileDefinitionDescription(filenameInferredTypes)}", new[] { nameof(content) });
        }

        // If it's an xml file check for script tags
        var extensionAliases = contentInferredTypes.SelectMany(e => e.Extensions)
            .Union(filenameInferredTypes.SelectMany(e => e.Extensions))
            .Union(new string[] { extension.ToLowerInvariant() });
        if(extensionAliases.Intersect(KnownXmlFileTypes).Any()) {
            // Take the first 1000 characters, it's a sanity check, not anti-virus
            var filecontent = Encoding.UTF8.GetString(content.Take(1000).ToArray()); 
            if(filecontent.IndexOf("<script", StringComparison.InvariantCultureIgnoreCase) >= 0) {
                yield return new ValidationResult("Provided file is an XML filetype with protected tags", new[] { nameof(content) });
            }
        }
    }

    /// <summary>
    /// Determine if the system is running in a WebAssembly runtime, allowing logic which is 
    /// conditional on running on only the client or the server.  
    /// </summary>
    /// <remarks>
    /// This doesn't seem to be great, but the advice online didn't work.  String comparison seems 
    /// to work, but the underlying `Enum` doesn't contain the actual value.  Presumably the enum 
    /// is compiled in differently and contains the additional type?
    /// </remarks>
    private static bool WebAssemblyRuntime => 
        RuntimeInformation.OSArchitecture.ToString().Equals("WASM", StringComparison.OrdinalIgnoreCase);
        // RuntimeInformation.IsOSPlatform(OSPlatform.Create("WEBASSEMBLY")); -> doesn't seem to work

    /// <summary>
    /// Get the file type description to populate error messages.
    /// </summary>
    private static string GetFileDefinitionDescription(IEnumerable<FileTypeDefinition> fileTypes)
    {
        var description = fileTypes.FirstOrDefault(f => !string.IsNullOrEmpty(f.Description))?.Description;
        return description ?? fileTypes.First().Extensions.FirstOrDefault() ?? "unknown";
    }

    /// <summary>
    /// File extensions that will be rejected regardless of the whitelist settings
    /// </summary>
    private readonly List<string> FileTypeBlacklist = new();

    /// <summary>
    /// Some files we'd like to reject based off their file extensions, but can't reject off magic bytes becuase they share these with other file types eg docx, zip, jar and apk
    /// </summary>
    private readonly List<string> ExtensionOnlyBlacklist = new();

    /// <summary>
    /// XML file types that are checked for script tags
    /// </summary>
    private List<string> KnownXmlFileTypes { get; set; } = new() { "xml", "html", "svg" };

    /// <summary>
    /// Regex that identifies non-letter characters that would not be valid in a Unicode filename.
    /// </summary>
    private const string InvalidUnicodeCharacterRegex = @"[^\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nd}\-_\.]";

    /// <summary>
    /// Regex that identifies non-letter characters that would not be valid in an Ascii filename.
    /// </summary>
    private const string InvalidAsciiCharacterRegex = @"[^a-zA-Z0-9\-_\.]";

    private FileTypeDefinitionSource fileService;

    private FileValidationOptions Options { get; set; }

}
