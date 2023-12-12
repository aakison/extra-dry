namespace ExtraDry.Core;

/// <summary>
/// Options for the file validator service. This is used to configure the file validator service.
/// </summary>
public interface IFileValidationOptions
{
    /// <summary>
    /// A list of extensions that will form the blacklist of file types that you would like to exclude from your system.
    /// Leave empty to use a default set
    /// </summary>
    ICollection<BlacklistFileType> ExtensionBlacklist { get; }

    /// <summary>
    /// The whitelist of file extensions that you would like your application to allow
    /// </summary>
    ICollection<string> ExtensionWhitelist { get; }

    /// <summary>
    /// Determines which characters <see cref="FileValidationService.CleanFilename(string)"/> 
    /// method will allow in the filename.  The default is <see cref="FilenameCharacters.UnicodeAlphaNumeric"/>
    /// which allows all Unicode alphanumeric characters, including diacritic marks (as in resumé).
    /// </summary>
    FilenameCharacters FileCleanerAllowedExtensionCharacters { get; }

    /// <summary>
    /// Determines which characters <see cref="FileValidationService.CleanFilename(string)"/> 
    /// method will allow in the file extension.  The default is <see cref="FilenameCharacters.AsciiAlphaNumeric"/>
    /// which allows all Ascii alphanumeric characters.
    /// </summary>
    FilenameCharacters FileCleanerAllowedNameCharacters { get; }

    /// <summary>
    /// Determines if the <see cref="FileValidationService.CleanFilename(string)"/> method will
    /// compress the filename by removing duplicate space, periods, and dashes.  The default is true.
    /// </summary>
    bool FileCleanerCompressFilename { get; }

    /// <summary>
    /// Determines if the <see cref="FileValidationService.CleanFilename(string)"/> method will
    /// convert the filename to lowercase.  The default is true.
    /// </summary>
    bool FileCleanerLowercase { get; }

    /// <summary>
    /// Determines if the <see cref="FileValidator" /> validates the file content.  The default is 
    /// <see cref="ValidationCondition.ServerSide" /> which runs content validation rules on the 
    /// server, but not on the blazor client.  This is recommended as the content validation rules
    /// requires a database of file signatures and mime types that could cause performance issues 
    /// if downloaded to the client.
    /// </summary>
    ValidationCondition ValidateContent { get; }

    /// <summary>
    /// Determines if the <see cref="FileValidator" /> validates file extensions.  The default is
    /// <see cref="ValidationCondition.Always" /> which checks extension on both the server and the
    /// client.  This is recommended as it is a simple check that can be performed on the client.
    /// </summary>
    ValidationCondition ValidateExtension { get; }

    /// <summary>
    /// Determines if the <see cref="FileValidator" /> validates filenames.  The default is
    /// <see cref="ValidationCondition.Always" /> which checks filenames on both the server and the
    /// client.  This is recommended as it is a simple check that can be performed on the client.
    /// </summary>
    ValidationCondition ValidateFilename { get; }
}
