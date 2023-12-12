using System.Reflection;

namespace ExtraDry.Core.Tests;

public class FileValidationTests {

    public FileValidationTests()
    {
        options = new FileValidationOptions() {
            CheckMagicBytesAndMimes = true,
            FileDatabaseLocation = "FileUpload/FileDatabase.json"
        };

        service = new FileValidationService(options);

        validator = new FileValidator(service);
    }

    [Theory]
    [InlineData("good.txt")] // basic correctness
    [InlineData("MixedCase.txt")] // mixed case
    [InlineData("Caractères-accentués-Sedím-na-stole-a-moja-výška-mi-umožňuje-pohodlne-čítať-knihu.txt")] // extended characters
    [InlineData("archive.tar.gz")] // multiple extensions.
    public void DoesNotAlterUnicodeFileNames(string filename)
    {
        var options = new FileValidationOptions() {
            FileCleanerAllowedNameCharacters = FilenameCharacters.UnicodeAlphaNumeric,
            FileCleanerAllowedExtensionCharacters = FilenameCharacters.All,
            FileCleanerLowercase = false,
        };
        var service = new FileValidationService(options);

        var clean = service.CleanFilename(filename);

        Assert.Equal(filename, clean);
    }

    [Theory]
    [InlineData("Bad!Name.txt")]
    [InlineData("Resumé!.txt")] // extended characters
    public void InvalidUnicodeFilename(string filename)
    {
        var options = new FileValidationOptions() {
            FileCleanerAllowedNameCharacters = FilenameCharacters.UnicodeAlphaNumeric,
            FileCleanerAllowedExtensionCharacters = FilenameCharacters.All,
            FileCleanerLowercase = false,
            ValidateContent = ValidationCondition.Never,
        };
        var service = new FileValidationService(options);

        var errors = service.ValidateFile(filename, "");

        Assert.Single(errors);
    }

    [Theory]
    [InlineData("good.txt")] // basic correctness
    [InlineData("MixedCase.txt")] // mixed case
    [InlineData("archive.tar.gz")] // multiple extensions.
    public void DoesNotAlterAsciiFileNames(string inputFilename)
    {
        var options = new FileValidationOptions() {
            FileCleanerAllowedNameCharacters = FilenameCharacters.AsciiAlphaNumeric,
            FileCleanerAllowedExtensionCharacters = FilenameCharacters.All,
            FileCleanerLowercase = false,
        };
        var service = new FileValidationService(options);

        var clean = service.CleanFilename(inputFilename);

        Assert.Equal(inputFilename, clean);
    }

    [Theory]
    [InlineData("Bad!Name.txt")]
    [InlineData("Resumé.txt")]
    public void InvalidAsciiFilename(string filename)
    {
        var options = new FileValidationOptions() {
            FileCleanerAllowedNameCharacters = FilenameCharacters.AsciiAlphaNumeric,
            FileCleanerAllowedExtensionCharacters = FilenameCharacters.All,
            FileCleanerLowercase = false,
            ValidateContent = ValidationCondition.Never,
        };
        var service = new FileValidationService(options);

        var errors = service.ValidateFile(filename, "");

        Assert.Single(errors);
    }

    [Theory]
    [InlineData("Ascii.txt")]
    [InlineData("Unicodé.txt")]
    [InlineData("!Leet$.txt")]
    public void CanConfigureForBadFilenames(string filename)
    {
        var options = new FileValidationOptions() {
            FileCleanerAllowedNameCharacters = FilenameCharacters.All,
            FileCleanerAllowedExtensionCharacters = FilenameCharacters.All,
            FileCleanerLowercase = false,
            ValidateContent = ValidationCondition.Never,
        };
        var service = new FileValidationService(options);

        var errors = service.ValidateFile(filename, "");

        Assert.Empty(errors);
    }

    [Theory]
    [InlineData("file1.test-1.txt", "file1.test-1.txt")]
    [InlineData("file1-.test-1.txt", "file1.test-1.txt")]
    [InlineData("123___sad---.html", "123_sad.html")]
    [InlineData("Caractères[]@%$#&().txt", "Caractères.txt")]
    [InlineData("Caractères[]@%$#&()sifd.txt", "Caractères-sifd.txt")]
    [InlineData("[]@%$#&()Caractèressifd.txt", "Caractèressifd.txt")]
    [InlineData("Sedím na stole a moja výška mi umožňuje pohodlne čítať knihu.txt", "Sedím-na-stole-a-moja-výška-mi-umožňuje-pohodlne-čítať-knihu.txt")]
    [InlineData("_siudh_.txt", "_siudh.txt")]
    [InlineData("-siudh-.txt", "siudh.txt")]
    [InlineData("-21938721309781231content.txt", "21938721309781231content.txt")]
    public void AltersFileNameToCorrect(string inputFilename, string expected)
    {
        var service = new FileValidationService(new FileValidationOptions() {
            
        });

        var clean = service.CleanFilename(inputFilename);

        Assert.Equal(expected, clean);
    }

    [Theory(Skip = "Refactor to remove file dependency")]
    [InlineData("txt.txt", "text/plain")]
    [InlineData("jpg.jpg", "image/jpeg")]
    [InlineData("png.png", "image/png")]
    [InlineData("rtf.rtf", "text/rtf")]
    [InlineData("word.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")] // interesting use case. .docx magic bytes = .zip magic bytes = .apk magic bytes
    [InlineData("zip.zip", "application/zip")] // interesting use case. .docx magic bytes = .zip magic bytes = .apk magic bytes
    [InlineData("Tiff.tiff", "image/tiff")]
    [InlineData("doc.doc", "application/msword")]
    [InlineData("mp4.mp4", "audio/mp4")]
    [InlineData("mp4.mp4", "audio/aac")]
    [InlineData("mp4.mp4", "video/mp4")]
    [InlineData("html.html", "text/html")]
    public void ValidToUploadFiles(string filename, string mime)
    {
        var fileBytes = File.ReadAllBytes($"FileUpload/SampleFiles/{filename}");

        validator.ValidateFile(filename, mime, fileBytes);

        Assert.True(validator.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("!bat.txt")]
    [InlineData("j%pg.txt")]
    [InlineData("rtf-.rtf")] // False positive, is actually valid.
    [InlineData("te1--xt.txt")] // False positive, is actually valid.
    [InlineData("text")]
    [InlineData("text.$$$")]
    public void InvalidFilenameWithDefaultOptions(string filename)
    {
        var validator = OptionedFileValidator(config => {
                config.ValidateContent = ValidationCondition.Never;
            });

        validator.ValidateFile(filename, "text/plain");

        Assert.False(validator.IsValid);
    }

    //[Theory]
    //[InlineData(SampleFiles.GoodDocxFileKey, "jpg.jpg")]
    ////[InlineData("file.txt", "png.png", "text/plain")]
    //public void ExtensionDoesNotMatchMimeType(int sample, string badFilename)
    //{
    //    var options = new FileValidationOptions() {
    //        ValidateContent = ValidationCondition.Always,
    //        ValidateExtension = ValidationCondition.Always,
    //        ValidateFilename = ValidationCondition.Always,
    //    };
    //    options.ExtensionBlacklist.Add(new BlacklistFileType { Extension = "jpg", CheckType = "image/jpeg" });
    //    var service = new FileValidationService(options);
    //    var file = SampleFiles.GetFile(sample);

    //    var errors = service.ValidateFile(badFilename, file.MimeType, file.Content);

    //    Assert.Single(errors);
    //}

    [Theory(Skip = "Refactor to remove file dependency")]
    [InlineData("doc.docx", "word.docx", "image/jpeg")]
    public void MismatchingNameAndMime(string filename, string filepath, string mime)
    {
        var fileBytes = File.ReadAllBytes($"FileUpload/SampleFiles/{filepath}");

        validator.ValidateFile(filename, mime, fileBytes);

        Assert.False(validator.IsValid);
        var exception = Assert.Throws<ValidationException>(() => validator.ThrowIfInvalid());
        Assert.Contains("filename and mime type do not match", exception.Message);
    }

    [Theory]
    [InlineData(SampleFiles.GoodBatFileKey)]
    //[InlineData("exe.exe", "exe.exe", "application/x-dosexec")]
    //[InlineData("zip.jar", "zip.zip", "application/java-archive")]
    //[InlineData("file.apk", "word.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
    public void BlacklistFileType(int filekey)
    {
        var file = SampleFiles.GetFile(filekey);

        validator.ValidateFile(file.Filename, file.MimeType, file.Content);

        Assert.False(validator.IsValid);
        var exception = Assert.Throws<ValidationException>(() => validator.ThrowIfInvalid());
    }

    [Theory(Skip = "Refactor to remove file dependency")]
    [InlineData("html.html", "htmlScript.html", "textScript/html")] // Has script tags
    public void XmlFileWithScriptFileType(string filename, string filepath, string mime)
    {
        var fileBytes = File.ReadAllBytes($"FileUpload/SampleFiles/{filepath}");

        validator.ValidateFile(filename, mime, fileBytes);

        Assert.False(validator.IsValid);
        Assert.Throws<ValidationException>(() => validator.ThrowIfInvalid());
    }

    [Fact]
    public void BlacklistBlocksExtension()
    {
        var options = new FileValidationOptions() {
            ExtensionWhitelist = new List<string> { "txt" }, // even if on whitelist...
            ExtensionBlacklist = new List<BlacklistFileType>() { new() { Extension = "txt" } },
            ValidateExtension = ValidationCondition.Always,
            ValidateContent = ValidationCondition.Never,
            ValidateFilename = ValidationCondition.Never,
        };
        var service = new FileValidationService(options);
        var validator = new FileValidator(service);
        var file = SampleFiles.GoodTextFile;

        validator.ValidateFile(file.Filename, file.MimeType, file.Content);

        Assert.False(validator.IsValid);
        Assert.Throws<ValidationException>(validator.ThrowIfInvalid);
    }

    [Fact]
    public void BlacklistCanBeIgnoredByConfiguration()
    {
        // same as above but BlacklistBlocksExtension but don't validate extension.
        var options = new FileValidationOptions() {
            ExtensionWhitelist = new List<string> { "txt" }, // even if on whitelist...
            ExtensionBlacklist = new List<BlacklistFileType>() { new() { Extension = "txt" } },
            ValidateExtension = ValidationCondition.Never,
            ValidateContent = ValidationCondition.Never,
            ValidateFilename = ValidationCondition.Never,
        };
        var service = new FileValidationService(options);
        var validator = new FileValidator(service);
        var file = SampleFiles.GoodTextFile;

        validator.ValidateFile(file.Filename, file.MimeType, file.Content);

        Assert.True(validator.IsValid);
    }


    [Theory(Skip = "Refactor to remove file dependency")]
    [InlineData("docx", "word.docx", "zip.zip", "application/zip")]
    public void NameOnlyBlacklistDoesntCheckBytes(string blackListFileExtension, string filepath, string filename, string mime)
    {
        options = new FileValidationOptions() {
            ExtensionBlacklist = new List<BlacklistFileType>() { new() { Extension = blackListFileExtension, CheckType = CheckType.FilenameOnly } },
            CheckMagicBytesAndMimes = true,
            FileDatabaseLocation = "FileUpload/FileDatabase.json"
        };
        service = new FileValidationService(options);
        validator = new FileValidator(service);
        var fileBytes = File.ReadAllBytes($"FileUpload/SampleFiles/{filepath}");

        validator.ValidateFile(filename, mime, fileBytes);

        Assert.True(validator.IsValid);
    }


    [Theory(Skip = "Refactor to remove file dependency")]
    [InlineData("exe.exe", "exe.jpg", "image/jpg")]
    [InlineData("exe.exe", "exe.jpg", "application/x-dosexec")]
    public void ClientSideConfigDoesntCheckBytes(string filepath, string filename, string mime)
    {
        options = new FileValidationOptions() {
            CheckMagicBytesAndMimes = false,
        };
        service = new FileValidationService(options);
        validator = new FileValidator(service);
        var fileBytes = File.ReadAllBytes($"FileUpload/SampleFiles/{filepath}");

        validator.ValidateFile(filename, mime, fileBytes);

        Assert.True(validator.IsValid);
    }

    [Theory(Skip = "Refactor to remove file dependency")]
    [InlineData("jpg", "exe.exe", "exe.jpg", "image/jpg")]
    [InlineData("exe", "exe.exe", "exe.exe", "application/x-dosexec")]
    public void ClientSideConfigDoesRejectByName(string blackListFileExtension, string filepath, string filename, string mime)
    {
        options = new FileValidationOptions() {
            ExtensionBlacklist = new List<BlacklistFileType>() { new() { Extension = blackListFileExtension, CheckType = CheckType.FilenameOnly } },
            CheckMagicBytesAndMimes = false,
            ValidateContent = ValidationCondition.Never,
            ValidateExtension = ValidationCondition.Always,
        };
        service = new FileValidationService(options);
        validator = new FileValidator(service);
        var fileBytes = File.ReadAllBytes($"FileUpload/SampleFiles/{filepath}");

        validator.ValidateFile(filename, mime, fileBytes);

        Assert.False(validator.IsValid);
        var exception = Assert.Throws<ValidationException>(() => validator.ThrowIfInvalid());
    }

    [Theory(Skip = "Refactor to remove file dependency")]
    [InlineData("exe.exe", "exe.jpg", "image/jpg")]
    [InlineData("exe.exe", "exe.html", "application/x-dosexec")]
    public void ClientSideConfigDoesAcceptValidFilenames(string filepath, string filename, string mime)
    {
        options = new FileValidationOptions() {
            CheckMagicBytesAndMimes = false,
            ValidateContent = ValidationCondition.Never,
        };
        service = new FileValidationService(options);
        validator = new FileValidator(service);
        var fileBytes = File.ReadAllBytes($"FileUpload/SampleFiles/{filepath}");

        validator.ValidateFile(filename, mime, fileBytes);

        Assert.True(validator.IsValid);
    }

    private FileValidationService service;

    private FileValidationOptions options;

    private FileValidator validator;

    private static FileValidator OptionedFileValidator(Action<FileValidationOptions> config)
    {
        var service = OptionedFileValidationService(config);
        return new FileValidator(service);
    }

    private static FileValidationService OptionedFileValidationService(Action<FileValidationOptions> config)
    {
        var options = new FileValidationOptions();
        config(options);
        return new FileValidationService(options);
    }

}
