using NHyphenator;
using NUnit.Framework;

namespace NHypenator.Tests
{
	[TestFixture]
	public class HypenatorTests
	{
		readonly Hypenator hypenator = new Hypenator(HypenatePatternsLanguage.EnglishUs, "•");

		[Test]
		public void PatternsTest()
		{
			Assert.AreEqual("sub•di•vi•sion", hypenator.HyphenateText("subdivision"));
			Assert.AreEqual("cre•ative", hypenator.HyphenateText("creative"));
			Assert.AreEqual("dis•ci•plines", hypenator.HyphenateText("disciplines"));
		}		
		
		[Test]
		public void ExceprionTest()
		{
			Assert.AreEqual("phil•an•thropic", hypenator.HyphenateText("philanthropic"));
		}

		[Test]
		public void ChangeSymbolTest()
		{
			var hypenator1 = new Hypenator(HypenatePatternsLanguage.EnglishUs, "&shy;");
			Assert.AreEqual("dis&shy;ci&shy;plines", hypenator1.HyphenateText("disciplines"));
		}
	}
}