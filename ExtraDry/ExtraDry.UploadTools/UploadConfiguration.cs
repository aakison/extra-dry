using System;
using System.Collections.Generic;
using System.Text;

namespace ExtraDry.UploadTools {
    public class UploadConfiguration {
        public List<string> ExtensionWhitelist { get; set; } = new List<string>();

        public List<FileTypeDefinition> FileDefinitions { get; set; } = new List<FileTypeDefinition>();

        public FileDefinitionBehaviour FileDefinitionBehaviour { get; set; } = FileDefinitionBehaviour.Add;

        public List<string> ExtensionBlacklist { get; set; } = new List<string>();
    }

    public enum FileDefinitionBehaviour
    {
        /// <summary>
        /// The File Definitions provided alongside this value will be used to add to the default ones in the file checker.
        /// </summary>
        Add,

        /// <summary>
        /// The File Definitions provided alongside this value will be used to replace the default ones in the file checker.
        /// </summary>
        Replace
    }

    /// <summary>
    /// An entity representing an item in the file blacklist. It contains its file extension and how to check the file.
    /// </summary>
    public class BlacklistFileType {
        /// <summary>
        /// The file extension to blacklist
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// The method to check the file extension. By default will check both filename and magic bytes.
        /// </summary>
        public CheckType CheckType { get; set; } = CheckType.BytesAndFilename;
    }

    public enum CheckType
    {
        /// <summary>
        /// For this extension, check the filename as well as the magic bytes. This is the default behaviour
        /// </summary>
        BytesAndFilename,

        /// <summary>
        /// For this extension, don't reject based off magic bytes. 
        /// </summary>
        /// <remarks>Used where a shared magic byte would reject a file you want. Eg Reject .apk, but don't reject .docx</remarks>
        FilenameOnly
    }
}
