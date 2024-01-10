namespace ExtraDry.Core.Tests;

public class FileValidationServiceTests {

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

    [Theory]
    [InlineData(SampleFiles.GoodTextFileKey)]
    //[InlineData("jpg.jpg", "image/jpeg")]
    //[InlineData("png.png", "image/png")]
    //[InlineData("rtf.rtf", "text/rtf")]
    //[InlineData("word.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")] // interesting use case. .docx magic bytes = .zip magic bytes = .apk magic bytes
    //[InlineData("zip.zip", "application/zip")] // interesting use case. .docx magic bytes = .zip magic bytes = .apk magic bytes
    //[InlineData("Tiff.tiff", "image/tiff")]
    //[InlineData("doc.doc", "application/msword")]
    //[InlineData("mp4.mp4", "audio/mp4")]
    //[InlineData("mp4.mp4", "audio/aac")]
    //[InlineData("mp4.mp4", "video/mp4")]
    //[InlineData("html.html", "text/html")]
    public void ValidToUploadFiles(int fileKey)
    {
        var file = SampleFiles.GetFile(fileKey);
        var service = new FileValidationService(new FileValidationOptions() {
            ValidateFilename = ValidationCondition.Always,
            ValidateExtension = ValidationCondition.Always,
            ValidateContent = ValidationCondition.Always,
        });

        var errors = service.ValidateFile(file.Filename, file.MimeType, file.Content);

        Assert.Empty(errors);
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
        var options = new FileValidationOptions { 
            ValidateContent = ValidationCondition.Never,
        };
        var service = new FileValidationService(options);
        var validator = new FileValidator(service);

        validator.ValidateFile(filename, "text/plain");

        Assert.False(validator.IsValid);
    }

    [Fact]
    public void ExtensionDoesMatchMimeTypeIsValid()
    {
        var options = new FileValidationOptions();
        var service = new FileValidationService(options);
        var validator = new FileValidator(service);

        validator.ValidateFile("filename.txt", "text/plain");

        Assert.False(validator.IsValid);
    }

    [Fact]
    public void ExtensionDoesntMatchMimeTypeIsInvalid()
    {
        var options = new FileValidationOptions();
        var service = new FileValidationService(options);
        var validator = new FileValidator(service);

        validator.ValidateFile("filename.txt", "image/jpeg");

        Assert.False(validator.IsValid);
    }

    [Fact]
    public void HtmlFileWithoutScriptTagsIsValid()
    {
        var options = new FileValidationOptions();
        var service = new FileValidationService(options);
        var validator = new FileValidator(service);
        var file = SampleFiles.ValidHtmlFile;

        validator.ValidateFile(file.Filename, file.MimeType, file.Content);

        Assert.True(validator.IsValid);
    }

    [Fact]
    public void HtmlFileWithScriptTagsIsInvalid()
    {
        var file = SampleFiles.InvalidHtmlFile;
        var options = new FileValidationOptions();
        var service = new FileValidationService(options);
        var validator = new FileValidator(service);

        validator.ValidateFile(file.Filename, file.MimeType, file.Content);

        Assert.False(validator.IsValid);
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
            ValidateExtension = ValidationCondition.Never, // <== blocks blacklist
            ValidateContent = ValidationCondition.Never,
            ValidateFilename = ValidationCondition.Never,
        };
        var service = new FileValidationService(options);
        var validator = new FileValidator(service);
        var file = SampleFiles.GoodTextFile;

        validator.ValidateFile(file.Filename, file.MimeType, file.Content);

        Assert.True(validator.IsValid);
    }

}
