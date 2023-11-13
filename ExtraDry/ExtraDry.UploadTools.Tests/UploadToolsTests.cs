using Xunit;

namespace ExtraDry.UploadTools.Tests {
    public class UploadToolsTests {

        public UploadToolsTests()
        {
            var testConfig = new UploadConfiguration() { ExtensionWhitelist = new List<string>{"txt", "jpg", "png", "rtf", "docx", "docm", "tiff", "doc", "mp4"} };
            UploadTools.ConfigureUploadRestrictions(testConfig);
        }

        [Theory]
        [InlineData("good.txt")]
        [InlineData("Resumè4.docx")]
        [InlineData("Caractères-accentués.txt")]
        [InlineData("Sedím-na-stole-a-moja-výška-mi-umožňuje-pohodlne-čítať-knihu.txt")]
        
        public void DoesNotAlterGoodFileNames(string inputFilename)
        {
            var clean = UploadTools.CleanFilename(inputFilename);

            Assert.Equal(inputFilename, clean);
        }

        [Theory]
        [InlineData("file1.test-1.txt", "file1-test-1.txt")]
        [InlineData("file1-.test-1.txt", "file1-test-1.txt")]
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
        [InlineData("text.txt")]
        [InlineData("jpg.jpg")]
        [InlineData("png.png")]
        [InlineData("rtf.rtf")]
        [InlineData("word.docx")]
        [InlineData("Tiff.tiff")]
        [InlineData("doc.doc")]
        [InlineData("mp4.mp4")]
        public async Task ValidToUploadFiles(string filename)
        {
            var fileBytes = File.ReadAllBytes($"SampleFiles/{filename}");

            var canUpload = await UploadTools.CanUpload(filename, "unused", fileBytes);

            Assert.True(canUpload);
        }

        [Theory]
        [InlineData("bat.bat", "bat.bat")] // this is and should be rejected, but is not in the library.
        [InlineData("!bat.bat", "bat.bat")]
        [InlineData("exe.exe", "exe.exe")]
        [InlineData("file.txt", "exe.exe")]
        // xml with script tags, mismatching content and file
        public async Task InvalidToUploadFiles(string filename, string filepath)
        {
            var fileBytes = File.ReadAllBytes($"SampleFiles/{filepath}");

            var canUpload = await UploadTools.CanUpload(filename, "unused", fileBytes);

            Assert.False(canUpload);
        }
    }
}
