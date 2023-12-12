using System.Reflection;
using System.Text;

namespace ExtraDry.Core.Tests;

public class SampleFile
{
    public string Filename { get; set; } = string.Empty;

    public string MimeType { get; set; } = string.Empty;

    public byte[] Content { get; set; } = Array.Empty<byte>();

}

public class SampleFiles
{
    public static SampleFile GetFile(int fileId) => fileId switch {
        GoodTextFileKey => GoodTextFile,
        GoodBatFileKey => GoodBatFile,
        GoodDocxFileKey => GoodDocxFile,
        _ => throw new ArgumentException($"Unknown file name {fileId}", nameof(fileId)),
    };

    public const int GoodTextFileKey = 1;

    public const int GoodBatFileKey = 2;

    public const int GoodDocxFileKey = 3;

    public static SampleFile GoodBatFile => new() {
        Filename = "test.bat",
        MimeType = "application/octet-stream",
        Content = Encoding.UTF8.GetBytes("echo hello world"),
    };

    public static SampleFile GoodTextFile => new() {
        Filename = "test.txt",
        MimeType = "text/plain",
        Content = Encoding.UTF8.GetBytes("Hello, World!"),
    };

    public static SampleFile GoodDocxFile => new() {
        Filename = "test.docx",
        MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        Content = GetBytes("test.docx"),
    };

    private static byte[] GetBytes(string filename)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream($"ExtraDry.Core.Tests.FileValidation.SampleFiles.{filename}");
        using var reader = new StreamReader(stream!);
        using var memoryStream = new MemoryStream();
        stream!.CopyTo(memoryStream);
        var content = memoryStream.ToArray();
        return content;
    }

}
