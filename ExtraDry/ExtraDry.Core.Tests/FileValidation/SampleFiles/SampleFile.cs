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
    public static SampleFile GetFile(string name) => name switch {
        "text.txt" => TextFile,
        "bat.bat" => BatFile,
        _ => throw new ArgumentException($"Unknown file name {name}", nameof(name)),
    };

    public static SampleFile BatFile => new() {
        Filename = "test.bat",
        MimeType = "application/octet-stream",
        Content = Encoding.UTF8.GetBytes("echo hello world")
    };

    public static SampleFile TextFile => new() {
        Filename = "test.txt",
        MimeType = "text/plain",
        Content = Encoding.UTF8.GetBytes("Hello, World!")
    };

}
