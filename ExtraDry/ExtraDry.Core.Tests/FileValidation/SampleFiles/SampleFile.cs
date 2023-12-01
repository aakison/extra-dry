using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraDry.Core.Tests;

public class SampleFile
{
    public string Filename { get; set; } = string.Empty;

    public string MimeType { get; set; } = string.Empty;

    public byte[] Content { get; set; } = Array.Empty<byte>();

}

public class SampleFiles
{
    public static SampleFile BatFile => new SampleFile {
        Filename = "test.bat",
        MimeType = "application/octet-stream",
        Content = Encoding.UTF8.GetBytes("echo hello world")
    };

}
