using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExtraDry.UploadTools {
    public static class UploadTools{

        /// <summary>
        /// File extensions that will be rejected regardless of the whitelist settings
        /// </summary>
        private readonly static List<string> ExtensionBlacklist = new List<string> { 
            "asp", "aspx", "config", "ashx", "asmx", "aspq", "axd", "cshtm", "cshtml", "rem", "soap", "vbhtm", "vbhtml", "asa", "cer", "shtml", // Pentester Identified, classified under "ASP"
            "jsp", "jspx", "jsw", "jsv", "jspf", "wss", "do", "action", // Pentester Identified, classified under "JSP"
            "bat", "bin", "cmd", "com", "cpl", "exe", "gadget", "inf1", "ins", "inx", "isu", "job", "jse", "lnk", "msc", "msi", "msp", "mst", "paf", "pif", "ps1", "reg", "rgs", "scr", "sct", "shb", "shs", "u3p", "vb", "vbe", "vbs", "vbscript", "ws", "wsf", "wsh", // Pentester Identified, classified under "General"
            "py", "jar", "go", "app", "scpt", "scptd", "apk"  // Additional filetypes
        };

        /// <summary>
        /// A consumer definable list of whitelisted file extensions. Note that any that also appear in the blacklist will be rejected
        /// </summary>
        private static List<string> ExtensionWhitelist { get; set; } = new List<string> {
            "txt", "jpg", "png", "jpeg"
        };

        private const string cleaningRegex = @"[^\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nd}\-_]";

        static FileService mimeTypes = new FileService();

        /// <summary>
        /// Call in startup of your application to configure the settings for the upload tools;
        /// </summary>
        public static void ConfigureUploadRestrictions(UploadConfiguration config)
        {
            if (config == null) {
                return;
            }
            if(config.ExtensionWhitelist != null && config.ExtensionWhitelist.Any()) {
                ExtensionWhitelist = config.ExtensionWhitelist;
            }
            // TODO - config will define additional file types possibly.
        }

        /// <summary>
        /// Given a filename, this will:
        ///  - Replace spaces with hyphens
        ///  - Remove invalid characters and replace with hyphens. 
        ///  - Ensure the file name begins with a valid character
        /// </summary>
        public static string CleanFilename(string filename)
        {
            // Sort out the filename part
            var fileOnly = Path.GetFileNameWithoutExtension(filename);

            // replace all the invalid characters
            var cleanedFilename = Regex.Replace(fileOnly, cleaningRegex, "-"); // Replace invalid characters with hyphens

            // collapse duplicate hyphens and underscores
            cleanedFilename = Regex.Replace(cleanedFilename, @"-+", "-"); // Collapse duplicate hyphens
            cleanedFilename = Regex.Replace(cleanedFilename, @"_+", "_"); // Collapse duplicate underscores

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

        public static async Task<bool> CanUpload(string filename, string mimetype, byte[] content)
        {
            // TODO - for each false return, throw a dry exception with details.

            // Filename Checking.
            // if no file name or no content, early exit.
            if(string.IsNullOrEmpty(filename) || filename.Length > 255 || content == null || !content.Any()) {
                return false;
            }

            // If the filename has bad characters in it and isn't already cleaned.
            if(filename != CleanFilename(filename)) {
                return false;
            }

            // File Extension Checking
            // TODO - How opinionated do we want to be? Do we allow consumers to define their blacklist?
            var fileExtension = Path.GetExtension(filename).TrimStart('.');

            // Outright reject executable file extensions - even if they're in the whitelist.
            if(ExtensionBlacklist.Contains(fileExtension, StringComparer.OrdinalIgnoreCase)) {
                return false;
            }

            // If the extension isn't in the whitelist, reject it
            if(!ExtensionWhitelist.Contains(fileExtension, StringComparer.OrdinalIgnoreCase)) {
                return false;
            }

            // Get the mime type and file type info from the file name and the content
            // We don't really use the one from the file name other than to check it matches the content
            // TODO - for a lot of types there are multiple types eg. mp4, check this is all good. Is order important? Means the mimetype may not match up. 
            // mimeTypes.ForName???
            // TODO - There are also shared magic bytes. eg pptx, docx, odt and ods have the same magic numebrs, and different mime types and extensions
            var filenameFileDefinition = mimeTypes.GetFileTypeFromFilename(filename);
            var mimeTypeFileDefinition = mimeTypes.GetFileTypeFromMime(mimetype);
            var magicByteFileDefinition = mimeTypes.GetFileTypeFromBytes(content); // TODO - Should we just take the first X bytes? Almost all file types are defined in the first few hundred bytes

            // If it's an unknown file type:
                // If we don't know the bytes, it's not flagged as dangerous.
                // If we don't know the type from the extension, we don't reject it because it's already through the file name whitelist and we know no better
            // So if both are null, we don't know anything more
            // If just the bytes mime is null, then there's nothing new to decide off
            // If just the file name mime is null, then unless the bytes are dangerous, we do nothing more. The filename filter should have already caught it otherwise.

            // If the magic bytes mime is in the blacklist, reject
            // TODO - Is there a risk that the magic bytes will match more than one file type, one of which is ok? need ot make sure that no magic bytes is not a match.
            if(magicByteFileDefinition != null && magicByteFileDefinition.SelectMany(e => e.Extensions).Intersect(ExtensionBlacklist, StringComparer.OrdinalIgnoreCase).Any()) {
                return false;
            }

            // If there's both a filename and a magic byte type file definition, and they don't match, reject
            if(magicByteFileDefinition != null && magicByteFileDefinition.Any() &&  // if there's a magic byte file definition
                filenameFileDefinition != null && filenameFileDefinition.Any() &&   // and a filename magic byte definition
                !filenameFileDefinition.SelectMany(f => f.MimeTypes).Intersect(magicByteFileDefinition.SelectMany(f => f.MimeTypes)).Any()) { // then they have to map
                return false;
            }

            // If there's both a filename and a mime type file definition, and they don't match, reject
            if(mimeTypeFileDefinition != null && mimeTypeFileDefinition.Any() &&        // If there's a mime type
                filenameFileDefinition != null && filenameFileDefinition.Any() &&       // And a file name type
                !filenameFileDefinition.SelectMany(f => f.MimeTypes).Intersect(mimeTypeFileDefinition.SelectMany(f => f.MimeTypes)).Any()) { // they have to match.
                return false;
            }


            // TODO - if it's an xml-type file, check there's no script or... was it data? message? whatever we got stung with in that pen test a year ago.
            // TODO - Check mime type in the request?

            return true;
        }
    }
}
