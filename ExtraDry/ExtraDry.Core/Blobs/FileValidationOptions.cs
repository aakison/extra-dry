﻿namespace ExtraDry.Core;

/// <summary>
/// Options for the file validator service. This is used to configure the file validator service.
/// </summary>
public class FileValidationOptions
{
    /// <summary>
    /// The whitelist of file extensions that are allowed. If this is empty, all extensions are
    /// allowed.
    /// </summary>
    public ICollection<string> ExtensionWhitelist { get; set; } = [
        "7z", "3ds", "3gp", "ai", "avi", "bak", "bmp", "bz2", "csv", "cxp", "doc", "docx", "dot",
        "dwg", "dwt", "dxf", "eml", "fpx", "gif", "gz", "heic", "html", "iam", "idw", "ifc",
        "iges", "ipt", "jpeg", "jpg", "key", "log", "m4a", "m4v", "md", "mht", "mov", "mp3", "mp4",
        "mpeg", "mpg", "msg", "nwc", "nwd", "nwf", "obj", "ods", "odt", "oft", "ogg", "pdf", "png",
        "ppt", "pptm", "pptx", "psd", "rar", "rec", "rfa", "rte", "rtf", "rvt", "shp", "stl",
        "tar", "tgz", "tif", "tiff", "ttd", "txt", "wav", "xls", "xlsm", "xlsx", "xltm", "zip"
    ];

    /// <summary>
    /// The blacklist of file extensions that are rejected.
    /// </summary>
    public ICollection<string> ExtensionBlacklist { get; set; } = [
        "asp", "aspx", "config", "ashx", "asmx", "aspq", "axd", "cshtm", "cshtml", "rem", "soap",
        "vbhtm", "vbhtml", "asa", "cer", "shtml", "jsp", "jspx", "jsw", "jsv", "jspf", "wss",
        "do", "action", "bat", "bin", "cmd", "com", "cpl", "exe", "gadget", "inf1", "ins", "inx",
        "isu", "job", "jse", "lnk", "msc", "msi", "msp", "mst", "paf", "pif", "ps1", "reg", "rgs",
        "scr", "sct", "shb", "shs", "u3p", "vb", "vbe", "vbs", "vbscript", "ws", "wsf", "wsh",
        "py", "go", "app", "scpt", "scptd", "apk", "jar", "ipa", "xap", "xpi", "crx", "oex"
    ];

    /// <summary>
    /// The list of extensions that are completely rejected based on the content of the file. The
    /// extension must also have a content definition provided in the <see
    /// cref="FileTypeDefinitions" /> list.
    /// </summary>
    public ICollection<string> ContentBlacklist { get; set; } = ["exe", "vbe"];

    /// <summary>
    /// If set, defines a file location to load the options from. Additional option settings are
    /// applied after the file is read.
    /// </summary>
    public string? OptionsFilepath { get; set; }

    /// <summary>
    /// A list of definitions for file types that you would like to check for when uploading a file
    /// Provides mappings between file extensions, mime types, and 'magic bytes' in the content
    /// that indicate the file type.
    /// </summary>
    public ICollection<FileTypeDefinition> FileTypeDefinitions { get; set; } = [
        new("exe", "application/x-dosexec", "Windows Executable (EXE)") {
            MagicBytes = {
                new MagicBytes {
                    Offset = 0,
                    Value = "MZ",
                    Type = MagicByteType.Content
                }
            }
        },
        new("vbe", "application/x-vbscript", "VBScript Encoded script") {
            MagicBytes = {
                new MagicBytes {
                    Offset = 0,
                    Value = "23407E5E",
                    Type = MagicByteType.Bytes,
                }
            }
        }
    ];

    /// <summary>
    /// Determines if the <see cref="FileValidator" /> validates the file content. The default is
    /// <see cref="ValidationCondition.ServerSide" /> which runs content validation rules on the
    /// server, but not on the blazor client. This is recommended as the content validation rules
    /// requires a database of file signatures and mime types that could cause performance issues
    /// if downloaded to the client.
    /// </summary>
    public ValidationCondition ValidateContent { get; set; } = ValidationCondition.ServerSide;

    /// <summary>
    /// Determines if the <see cref="FileValidator" /> validates file extensions. The default is
    /// <see cref="ValidationCondition.Always" /> which checks extension on both the server and the
    /// client. This is recommended as it is a simple check that can be performed on the client.
    /// </summary>
    public ValidationCondition ValidateExtension { get; set; } = ValidationCondition.Always;

    /// <summary>
    /// Determines if the <see cref="FileValidator" /> validates filenames. The default is <see
    /// cref="ValidationCondition.Always" /> which checks filenames on both the server and the
    /// client. This is recommended as it is a simple check that can be performed on the client.
    /// </summary>
    public ValidationCondition ValidateFilename { get; set; } = ValidationCondition.Always;

    /// <summary>
    /// Determines which characters <see cref="FileValidationService.CleanFilename(string)" />
    /// method will allow in the file extension. The default is <see
    /// cref="FilenameCharacters.AsciiAlphaNumeric" /> which allows all Ascii alphanumeric
    /// characters.
    /// </summary>
    public FilenameCharacters FileCleanerAllowedNameCharacters { get; set; } = FilenameCharacters.UnicodeAlphaNumeric;

    /// <summary>
    /// Determines which characters <see cref="FileValidationService.CleanFilename(string)" />
    /// method will allow in the filename. The default is <see
    /// cref="FilenameCharacters.UnicodeAlphaNumeric" /> which allows all Unicode alphanumeric
    /// characters, including diacritic marks (as in resumé).
    /// </summary>
    public FilenameCharacters FileCleanerAllowedExtensionCharacters { get; set; } = FilenameCharacters.AsciiAlphaNumeric;

    /// <summary>
    /// Determines if the <see cref="FileValidationService.CleanFilename(string)" /> method will
    /// compress the filename by removing duplicate space, periods, and dashes. The default is
    /// true.
    /// </summary>
    public bool FileCleanerCompressFilename { get; } = true;

    /// <summary>
    /// Determines if the <see cref="FileValidationService.CleanFilename(string)" /> method will
    /// convert the filename to lowercase. The default is true.
    /// </summary>
    public bool FileCleanerLowercase { get; set; } = true;

    /// <summary>
    /// The maximum length of a filename, longer files will not be valid.
    /// </summary>
    public int FileCleanerMaxLength { get; set; } = 255;
}
