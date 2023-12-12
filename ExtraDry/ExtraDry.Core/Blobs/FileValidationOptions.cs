﻿namespace ExtraDry.Core;

/// <summary>
/// Options for the file validator service. This is used to configure the file validator service.
/// </summary>
public class FileValidationOptions : IFileValidationOptions
{

    /// <inheritdoc/>
    public ICollection<string> ExtensionWhitelist { get; set; } = new List<string>() {
        "7z", "3ds", "3gp", "ai", "avi", "bak", "bmp", "bz2", "csv", "cxp", "doc", "docx", "dot",
        "dwg", "dwt", "dxf", "eml", "fpx", "gif", "gz", "heic", "html", "iam", "idw", "ifc", "iges",
        "ipt", "jpeg", "jpg", "key", "log", "m4a", "m4v", "md", "mht", "mov", "mp3", "mp4",
        "mpeg", "mpg", "msg", "nwc", "nwd", "nwf", "obj", "ods", "odt", "oft", "ogg", "pdf", "png",
        "ppt", "pptm", "pptx", "psd", "rar", "rec", "rfa", "rte", "rtf", "rvt", "shp", "stl", "tar",
        "tgz", "tif", "tiff", "ttd", "txt", "wav", "xls", "xlsm", "xlsx", "xltm", "zip"
    };

    /// <summary>
    /// Sets a value indicating whether the file service should be checking magic bytes and mime types.
    /// Set to true for server-side validation, and false for client side.
    /// </summary>
    public bool CheckMagicBytesAndMimes { get; set; }

    /// <summary>
    /// If <see cref="CheckMagicBytesAndMimes"/> is set to true, you can optionally set a location for a file database. If left blank, the default will be used.
    /// </summary>
    public string? FileDatabaseLocation { get; set; }

    /// <inheritdoc/>
    public ICollection<BlacklistFileType> ExtensionBlacklist { get; set; } = new List<BlacklistFileType>();

    /// <inheritdoc/>
    public ValidationCondition ValidateContent { get; set; } = ValidationCondition.ServerSide;

    /// <inheritdoc/>
    public ValidationCondition ValidateExtension { get; set; } = ValidationCondition.Always;

    /// <inheritdoc/>
    public ValidationCondition ValidateFilename { get; set; } = ValidationCondition.Always;

    /// <inheritdoc/>
    public FilenameCharacters FileCleanerAllowedNameCharacters { get; set; } = FilenameCharacters.UnicodeAlphaNumeric;

    /// <inheritdoc/>
    public FilenameCharacters FileCleanerAllowedExtensionCharacters { get; set; } = FilenameCharacters.AsciiAlphaNumeric;

    /// <inheritdoc/>
    public bool FileCleanerCompressFilename { get; } = true;

    /// <inheritdoc/>
    public bool FileCleanerLowercase { get; set; } = true;

}