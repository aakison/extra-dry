using System.Reflection;
using System.Text;

namespace ExtraDry.Core.Tests;

public class SampleFile
{
    public string Filename { get; set; } = string.Empty;

    public string MimeType { get; set; } = string.Empty;

    public byte[] Content { get; set; } = [];
}

public class SampleFiles
{
    public static SampleFile GetFile(int fileId) => fileId switch {
        GoodTextFileKey => ValidTextFile,
        GoodBatFileKey => ValidBatFile,
        GoodDocxFileKey => ValidDocxFile,
        _ => throw new ArgumentException($"Unknown file name {fileId}", nameof(fileId)),
    };

    public const int GoodTextFileKey = 1;

    public const int GoodBatFileKey = 2;

    public const int GoodDocxFileKey = 3;

    public static SampleFile ValidBatFile => new() {
        Filename = "test.bat",
        MimeType = "application/octet-stream",
        Content = Encoding.UTF8.GetBytes("echo hello world"),
    };

    public static SampleFile ValidTextFile => new() {
        Filename = "test.txt",
        MimeType = "text/plain",
        Content = Encoding.UTF8.GetBytes("Hello, World!"),
    };

    public static SampleFile ValidDocxFile => new() {
        Filename = "test.docx",
        MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        Content = GetBytes("test.docx"),
    };

    public static SampleFile ValidZipFile => new() {
        Filename = "test.zip",
        MimeType = "application/zip",
        Content = GetBytes("test.zip"),
    };

    public static SampleFile ValidJarFile => new() {
        Filename = "test.jar",
        MimeType = "application/java-archive",
        Content = GetBytes("test.jar"),
    };

    public static SampleFile ValidHtmlFile => new() {
        Filename = "test.html",
        MimeType = "text/html",
        Content = Encoding.UTF8.GetBytes(@"<!DOCTYPE html>
        <html>
          <body><h1>Hello, World!</h1></body>
        </html>")
    };

    public static SampleFile InvalidHtmlFile => new() {
        Filename = "test.html",
        MimeType = "text/html",
        Content = Encoding.UTF8.GetBytes(@"<!DOCTYPE html>
        <html>
          <body><h1>Hello, World!</h1></body>
          <script>alert('hello');</script>
        </html>")
    };

    private static byte[] GetBytes(string filename)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream($"ExtraDry.Core.Tests.Blobs.SampleFiles.{filename}");
        using var reader = new StreamReader(stream!);
        using var memoryStream = new MemoryStream();
        stream!.CopyTo(memoryStream);
        var content = memoryStream.ToArray();
        return content;
    }
}
