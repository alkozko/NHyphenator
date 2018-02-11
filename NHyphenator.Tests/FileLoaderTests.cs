using NHyphenator.Loaders;
using NUnit.Framework;

namespace NHyphenator.Tests
{
    public class FileLoaderTests
    {
        [Test]
        public void LoadPatternsTest()
        {
            var loader = new FilePatternsLoader("./Resources/test_pat.txt");
            var hyphenator = new Hyphenator(loader, "-");
            var hyphenateText = hyphenator.HyphenateText("перенос");
            Assert.AreEqual("пере-нос", hyphenateText);
        }


        [Test]
        public void LoadExceptionsTest()
        {
            var loader = new FilePatternsLoader("./Resources/test_pat.txt", "./Resources/test_ex.txt");
            var hyphenator = new Hyphenator(loader, "-");
            var hyphenateText = hyphenator.HyphenateText("перенос");
            Assert.AreEqual("пе-ре-нос", hyphenateText);
        }
    }
}