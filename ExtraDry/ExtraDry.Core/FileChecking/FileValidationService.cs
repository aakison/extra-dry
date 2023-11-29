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
    /// Whitelisted file extensions. Note that any that also appear in the blacklists will be rejected. The blacklist overrides the whitelist
    /// </summary>
    private List<string> ExtensionWhitelist { get; set; }

    /// <summary>
    /// XML file types that are checked for script tags
    /// </summary>
    private List<string> KnownXmlFileTypes { get; set; } = new() { "xml", "html", "svg" };

    /// <summary>
    /// Regex used to help clean the filenames.
    /// </summary>
    private const string cleaningRegex = @"[^\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nd}\-_\.]";

    private readonly FileService? fileService;

    private readonly bool CheckFileContent;


    public FileValidationService(FileValidationOptions config)
    {
        CheckFileContent = config.CheckMagicBytesAndMimes;
        if(CheckFileContent) {
            fileService = new FileService(config.FileDatabaseLocation!);
        }
        ExtensionWhitelist = new List<string>();
        ConfigureUploadRestrictions(config);
    }

    /// <summary>
    /// Call in startup of your application to configure the settings for the upload tools;
    /// Lists that aren't provided will be set to default values.
    /// </summary>
    internal void ConfigureUploadRestrictions(FileValidationOptions config)
    {
        if(config?.ExtensionWhitelist?.Count > 0) {
            ExtensionWhitelist = config.ExtensionWhitelist;
        }
        else {
            ExtensionWhitelist = new List<string> { "txt", "jpg", "png", "jpeg" };
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
    ///  - Ensure the file name begins with a valid character
    ///  - Trim invalid characters from start and end from filename
    /// </summary>
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
    /// - The magic bytes, filename and mimetype must match if present
    /// - If the file type is of a known xml type, it must not contain a script tag
    /// If all of these are true, then true is returned. Else, a <see cref="DryException"/> with details is thrown
    /// </summary>
    internal bool ValidateFile(string filename, string mimetype, byte[] content)
    {
        // if no file name or no content, early exit.
        if(string.IsNullOrEmpty(filename) || filename.Length > 255 || content == null || content.Length == 0) {
            throw new DryException("Provided file has no content");
        }

        // File Extension Checking
        var fileExtension = Path.GetExtension(filename).TrimStart('.');
        if(string.IsNullOrEmpty(fileExtension)) {
            throw new DryException("Provided filename contains an invalid extension", "File extension was not found");
        }

        // If the filename has bad characters in it and isn't already cleaned.
        if(filename != CleanFilename(filename)) {
            throw new DryException("Provided filename contained invalid characters", $"Filename was {filename}");
        }

        // Outright reject executable file extensions - even if they're in the whitelist.
        if(ExtensionOnlyBlacklist.Contains(fileExtension, StringComparer.OrdinalIgnoreCase)) {
            throw new DryException("Provided filename belongs to a forbidden filetype", $"File was of type \"{fileExtension}\"");
        }

        // If the extension isn't in the whitelist, reject it
        if(!ExtensionWhitelist.Contains(fileExtension, StringComparer.OrdinalIgnoreCase)) {
            throw new DryException("Provided filename belongs to a forbidden filetype", $"File was of type \"{fileExtension}\"");
        }

        // If we're not on the server side, this is the end of the parts we care about
        if(!CheckFileContent) {
            return true;
        }

        if(fileService == null) {
            throw new NullReferenceException("Upload service was misconfigured. File service was not found");
        }

        // Get the mime type and file type info from the file name and the content
        // We don't really use the one from the file name other than to check it matches the content
        var filenameFileDefinition = fileService.GetFileTypeFromFilename(filename);
        var mimeTypeFileDefinition = fileService.GetFileTypeFromMime(mimetype);
        var magicByteFileDefinition = fileService.GetFileTypeFromBytes(content);

        // If it's an unknown file type:
        // If we don't know the bytes, it's not flagged as dangerous.
        // If we don't know the type from the extension, we don't reject it because it's already through the file name whitelist and we know no better
        // If we don't know the mime type, then it's ok, mime types are client side and they might not know the mimetype themselves.

        // If the magic bytes filetype is in the bytes blacklist, reject
        var blacklistType = magicByteFileDefinition?.SelectMany(e => e.Extensions)?.Intersect(FileTypeBlacklist, StringComparer.OrdinalIgnoreCase)?.FirstOrDefault();
        if(blacklistType != null) {
            throw new DryException("Provided file content belongs to a forbidden filetype", $"Detected filetype was {blacklistType}");
        }

        // If there's both a filename and a magic byte type file definition, and they don't match, reject
        if(magicByteFileDefinition?.Count > 0 &&  // if there's a magic byte file definition
            filenameFileDefinition?.Count > 0 &&   // and a filename magic byte definition
            !filenameFileDefinition.SelectMany(f => f.MimeTypes).Intersect(magicByteFileDefinition.SelectMany(f => f.MimeTypes)).Any()) { // then they have to map
            throw new DryException("Provided file content and filename do not match", $"Content was {GetFileDefinitionDescription(magicByteFileDefinition)}, Filename was {GetFileDefinitionDescription(filenameFileDefinition)}");
        }

        // If there's both a filename and a mime type file definition, and they don't match, reject
        if((mimeTypeFileDefinition?.Count > 0) &&        // If there's a mime type 
            (filenameFileDefinition?.Count > 0) &&       // And a file name type
            !filenameFileDefinition.SelectMany(f => f.MimeTypes).Intersect(mimeTypeFileDefinition.SelectMany(f => f.MimeTypes)).Any()) { // they have to match.
            throw new DryException("Provided file name and mimetype do not match", $"Mime type was {GetFileDefinitionDescription(mimeTypeFileDefinition)}, Filename was {GetFileDefinitionDescription(filenameFileDefinition)}");
        }

        if(magicByteFileDefinition.SelectMany(e => e.Extensions).Union(filenameFileDefinition.SelectMany(e => e.Extensions)).Intersect(KnownXmlFileTypes).Any()) {
            // If it's an xml file check for script tags
            var filecontent = UTF8Encoding.UTF8.GetString(content.Take(1000).ToArray()); // Take the first 1000 characters, it's a sanity check, not anti-virus
            if(filecontent.IndexOf("<script", StringComparison.InvariantCultureIgnoreCase) >= 0) {
                throw new DryException("Provided file is an XML filetype with protected tags", "Script tags found");
            }
        }

        return true;
    }

    /// <summary>
    /// Get the file type description to populate error messages.
    /// </summary>
    private static string GetFileDefinitionDescription(List<FileTypeDefinition> fileTypes)
    {
        if(fileTypes?.Count == 0) {
            return "unknown";
        }
        var description = fileTypes.FirstOrDefault(f => !string.IsNullOrEmpty(f.Description))?.Description;
        return description ?? fileTypes.First().Extensions.FirstOrDefault() ?? "unknown";
    }
}
