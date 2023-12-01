using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtraDry.Core;

public class FileValidationService
{
    /// <summary>
    /// File extensions that will be rejected regardless of the whitelist settings
    /// </summary>
    private readonly List<string> FileTypeBlacklist = new();

    /// <summary>
    /// Some files we'd like to reject based off their file extensions, but can't reject off magic bytes becuase they share these with other file types eg docx, zip, jar and apk
    /// </summary>
    private readonly List<string> ExtensionOnlyBlacklist = new();

    /// <summary>
    /// Whitelisted file extensions. Note that blacklist extension duplicates will override this list.
    /// </summary>
    public ReadOnlyCollection<string> ExtensionWhitelist => new(extensionWhitelist);
    private List<string> extensionWhitelist;

    /// <summary>
    /// XML file types that are checked for script tags
    /// </summary>
    private List<string> KnownXmlFileTypes { get; set; } = new() { "xml", "html", "svg" };

    /// <summary>
    /// Regex used to help clean the filenames.
    /// </summary>
    private const string cleaningRegex = @"[^\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nd}\-_\.]";

    private FileTypeDefinitionSource fileService;

    private readonly bool CheckFileContent;

    public FileValidationService(FileValidationOptions config)
    {
        CheckFileContent = config.CheckMagicBytesAndMimes;
        if(CheckFileContent) {
            fileService = new FileTypeDefinitionSource(config.FileDatabaseLocation!);
        }
        else {
            fileService = new FileTypeDefinitionSource("");
        }
        extensionWhitelist = new List<string>(config.ExtensionWhitelist);
        ConfigureUploadRestrictions(config);

        ValidateContent = config.ValidateContent;
        ValidateExtension = config.ValidateExtension;
    }

    /// <inheritdoc cref="FileValidationOptions.ValidateContent" />
    public ValidationCondition ValidateContent { get; private set; }

    /// <inheritdoc cref="FileValidationOptions.ValidateExtension" />
    public ValidationCondition ValidateExtension { get; private set; }

    /// <summary>
    /// Call in startup of your application to configure the settings for the upload tools;
    /// Lists that aren't provided will be set to default values.
    /// </summary>
    private void ConfigureUploadRestrictions(FileValidationOptions config)
    {
        if(config.ExtensionWhitelist.Count > 0) {
            extensionWhitelist = config.ExtensionWhitelist;
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
    /// Given a filename, this will:
    ///  - Replace spaces with hyphens
    ///  - Remove invalid characters and replace with hyphens. 
    ///  - Ensure the filename begins with a valid character
    ///  - Trim invalid characters from start and end from filename
    /// </summary>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "It will access instance once configuration implemented.")]
    public string CleanFilename(string filename)
    {
        // Sort out the filename part
        var fileOnly = Path.GetFileNameWithoutExtension(filename);

        // replace all the invalid characters
        var cleanedFilename = Regex.Replace(fileOnly, cleaningRegex, "-"); // Replace invalid characters with hyphens

        // collapse duplicate hyphens and underscores
        cleanedFilename = Regex.Replace(cleanedFilename, @"-+", "-"); // Collapse duplicate hyphens
        cleanedFilename = Regex.Replace(cleanedFilename, @"_+", "_"); // Collapse duplicate underscores
        cleanedFilename = Regex.Replace(cleanedFilename, @"[-\.]{2,}", "."); // Collapse -.- combinations to a .

        // Remove trailing or leading hyphens
        cleanedFilename = cleanedFilename.Trim('-');
        cleanedFilename = cleanedFilename.TrimEnd('_');

        // Now the file extension, the only extensions that aren't alphanumeric are files that we likely should be rejecting
        // The addition of the hyphen as well allows for wordpress content and other similar proprietary but possible files.
        // https://www.file-extensions.org/extensions/begin-with-special-characters
        // https://www.file-extensions.org/filetype/extension/name/miscellaneous-files
        var extension = Path.GetExtension(filename).Trim('.');
        extension = Regex.Replace(extension, @"[^a-zA-Z0-9\-]", string.Empty);

        cleanedFilename = $"{cleanedFilename}.{extension}";
        return cleanedFilename;
    }

    /// <summary>
    /// Determines whether a file can be uploaded, by using the following rules
    /// - The filename must not contain no special characters
    /// - The filename must not not have an extension that is in our blacklist
    /// - The filename must be included in our whitelist
    /// - The magic bytes, filename and mime type must match if present
    /// - If the file type is of a known xml type, it must not contain a script tag
    /// If all of these are true, then true is returned. Else, a <see cref="DryException"/> with details is thrown
    /// </summary>
    internal IEnumerable<ValidationResult> ValidateFile(string filename, string mimetype, byte[]? content = null)
    {
        if(string.IsNullOrEmpty(filename)) {
            yield return new ValidationResult("Provided filename was null or empty");
        }

        if(filename.Length > 255) {
            yield return new ValidationResult("Provided filename was too long");
        }

        var extension = Path.GetExtension(filename).TrimStart('.');

        // If the filename has bad characters in it and isn't already cleaned.
        if(filename != CleanFilename(filename)) {
            yield return new ValidationResult($"Provided filename contained invalid characters, '{filename}'");
        }

        fileService ??= new();

        // Get the mime type and file type info from the filename and the content
        // We don't really use the one from the filename other than to check it matches the content
        var filenameInferredTypes = fileService.GetFileTypeFromFilename(filename);
        var mimeInferredTypes = fileService.GetFileTypeFromMime(mimetype);

        // If there's both a filename and a mime type file definition, and they don't match, reject
        if(mimeInferredTypes.Any() &&
            filenameInferredTypes.Any() &&
            !filenameInferredTypes.SelectMany(f => f.MimeTypes).Intersect(mimeInferredTypes.SelectMany(f => f.MimeTypes)).Any()) { 
            yield return new ValidationResult($"Provided filename and mime type do not match, mime type was {GetFileDefinitionDescription(mimeInferredTypes)}, and filename was {GetFileDefinitionDescription(filenameInferredTypes)}");
        }

        var extensionErrors = ValidateFileExtension(extension).ToList();
        var contentErrors = ValidateFileContent(content, filenameInferredTypes).ToList();
        foreach(var error in contentErrors.Union(extensionErrors)) {
            yield return error;
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

    private IEnumerable<ValidationResult> ValidateFileContent(byte[]? content, IEnumerable<FileTypeDefinition> filenameInferredTypes)
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
        var contentInferredTypes = fileService.GetFileTypeFromBytes(content);

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
        if(contentInferredTypes.SelectMany(e => e.Extensions).Union(filenameInferredTypes.SelectMany(e => e.Extensions)).Intersect(KnownXmlFileTypes).Any()) {
            // If it's an xml file check for script tags
            // Take the first 1000 characters, it's a sanity check, not anti-virus
            var filecontent = Encoding.UTF8.GetString(content.Take(1000).ToArray()); 
            if(filecontent.IndexOf("<script", StringComparison.InvariantCultureIgnoreCase) >= 0) {
                yield return new ValidationResult("Provided file is an XML filetype with protected tags", new[] { nameof(content) });
            }
        }
    }

    /// <summary>
    /// Determine if the system is running in a WebAssembly runtime, allowing logic which is 
    /// conditional on running on the client and/or the server.  
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

}
