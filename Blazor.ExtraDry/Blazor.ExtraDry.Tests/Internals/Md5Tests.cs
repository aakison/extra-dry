using Blazor.ExtraDry.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.ExtraDry.Tests.Internals {
    public class Md5Tests {

        [Theory]
        [InlineData("adrian@akison.com", "E5A7055CAB0FCBBAE38D8C1FB5840D03")]
        [InlineData("myemailaddress@example.com", "0BC83CB571CD1C50BA6F3E8A78EF1346")]
        [InlineData("shae.griffiths@gmail.com", "7286754AAD09B960126C09222700BD8A")]
        public void CorrectHash(string input, string expected)
        {
            var md5 = new MD5();

            var actual = md5.ComputeStringHash(input);

            Assert.Equal(expected, actual);
        }

    }
}
