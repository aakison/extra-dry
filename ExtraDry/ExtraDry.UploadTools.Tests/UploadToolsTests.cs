using ExtraDry.Core;
using Xunit;

namespace ExtraDry.UploadTools.Tests {
    public class UploadToolsTests {

        public UploadToolsTests()
        {
            var testConfig = new UploadConfiguration() { ExtensionWhitelist = new List<string>{"txt", "jpg", "png", "rtf", "docx", "docm", "tiff", "doc", "mp4", "html", "zip"} };
            UploadTools.ConfigureUploadRestrictions(testConfig);
        }

        [Theory]
        [InlineData("good.txt")]
        [InlineData("Resumè4.docx")]
        [InlineData("Caractères-accentués.txt")]
        [InlineData("Sedím-na-stole-a-moja-výška-mi-umožňuje-pohodlne-čítať-knihu.txt")]
        [InlineData("myArchive.tar.gz")]
        public void DoesNotAlterGoodFileNames(string inputFilename)
        {
            var clean = UploadTools.CleanFilename(inputFilename);

            Assert.Equal(inputFilename, clean);
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
            var clean = UploadTools.CleanFilename(inputFilename);

            Assert.Equal(expected, clean);
        }

        [Theory]
        [InlineData("text.txt", "text/plain")]
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
            var fileBytes = File.ReadAllBytes($"SampleFiles/{filename}");

            var canUpload = UploadTools.CanUpload(filename, mime, fileBytes);

            Assert.True(canUpload);
        }

        // TODO - Mime and filename, Filename and bytes, fail on filename without extension.
        //          Break up test types, ie invalid for what reason.


        [Theory]
        [InlineData("!bat.bat", "bat.bat", "text/plain")]
        [InlineData("te1--xt.txt", "text.txt", "text/plain")]
        [InlineData("text", "text.txt", "text/plain")]
        [InlineData("j%pg.jpg", "jpg.jpg", "image/jpeg")]
        [InlineData("!4png.png", "png.png", "image/png")]
        [InlineData("rtf-.rtf", "rtf.rtf", "text/rtf")]
        public void InvalidFileName(string filename, string filepath, string mime)
        {
            var fileBytes = File.ReadAllBytes($"SampleFiles/{filepath}");

            Assert.Throws<DryException>(() => UploadTools.CanUpload(filename, mime, fileBytes));

        }

        [Theory]
        [InlineData("doc.docx", "jpg.jpg", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [InlineData("file.txt", "exe.exe", "text/plain")]
        public void MismatchingNameAndBytes(string filename, string filepath, string mime)
        {
            var fileBytes = File.ReadAllBytes($"SampleFiles/{filepath}");

            Assert.Throws<DryException>(() => UploadTools.CanUpload(filename, mime, fileBytes));
        }

        [Theory]
        [InlineData("doc.docx", "word.docx", "image/jpeg")]
        [InlineData("file.txt", "text.txt", "image/jpeg")]
        public void MismatchingNameAndMime(string filename, string filepath, string mime)
        {
            var fileBytes = File.ReadAllBytes($"SampleFiles/{filepath}");

            Assert.Throws<DryException>(() => UploadTools.CanUpload(filename, mime, fileBytes));
        }

        [Theory]
        [InlineData("bat.bat", "bat.bat", "text/plain")]
        [InlineData("exe.exe", "exe.exe", "application/x-dosexec")]
        [InlineData("zip.jar", "zip.zip", "application/java-archive")]
        [InlineData("file.apk", "word.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        public void BlacklistFileType(string filename, string filepath, string mime)
        {
            var fileBytes = File.ReadAllBytes($"SampleFiles/{filepath}");

            Assert.Throws<DryException>(() => UploadTools.CanUpload(filename, mime, fileBytes));
        }

        [Theory]
        [InlineData("html.html", "htmlScript.html", "textScript/html")] // Has script tags
        public void XmlFileWithScriptFileType(string filename, string filepath, string mime)
        {
            var fileBytes = File.ReadAllBytes($"SampleFiles/{filepath}");

            Assert.Throws<DryException>(() => UploadTools.CanUpload(filename, mime, fileBytes));
        }




    }
}
