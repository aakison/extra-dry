using ExtraDry.Core.Internal;

namespace ExtraDry.Core.Tests.Internals;

public class Md5Tests
{
    [Theory]
    [InlineData("adrian@akison.com", "E5A7055CAB0FCBBAE38D8C1FB5840D03")]
    [InlineData("myemailaddress@example.com", "0BC83CB571CD1C50BA6F3E8A78EF1346")]
    [InlineData("shae.griffiths@gmail.com", "7286754AAD09B960126C09222700BD8A")]
    public void CorrectHash(string input, string expected)
    {
        var actual = MD5Core.GetHashString(input);

        Assert.Equal(expected, actual);
    }
}
