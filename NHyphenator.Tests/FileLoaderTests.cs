using NHyphenator.Loaders;
using Xunit;

namespace NHyphenator.Tests
{
    public class FileLoaderTests
    {
        [Fact]
        public void LoadPatternsTest()
        {
            var loader = new FilePatternsLoader("./Resources/test_pat.txt");
            var hyphenator = new Hyphenator(loader, "-");
            var hyphenateText = hyphenator.HyphenateText("перенос");
            Assert.Equal("пере-нос", hyphenateText);
        }


        [Fact]
        public void LoadExceptionsTest()
        {
            var loader = new FilePatternsLoader("./Resources/test_pat.txt", "./Resources/test_ex.txt");
            var hyphenator = new Hyphenator(loader, "-");
            var hyphenateText = hyphenator.HyphenateText("перенос");
            Assert.Equal("пе-ре-нос", hyphenateText);
        }
    }
}