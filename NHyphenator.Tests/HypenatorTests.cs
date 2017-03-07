﻿using NUnit.Framework;

namespace NHyphenator.Tests
{
	[TestFixture]
	public class HyphenatorTests
	{
		readonly Hyphenator hyphenator = new Hyphenator(HyphenatePatternsLanguage.EnglishUs, "•");

		[Test]
		public void PatternsTest()
		{
			Assert.AreEqual("sub•di•vi•sion", hyphenator.HyphenateText("subdivision"));
			Assert.AreEqual("cre•ative", hyphenator.HyphenateText("creative"));
			Assert.AreEqual("dis•ci•plines", hyphenator.HyphenateText("disciplines"));
		}		
		
		[Test]
		public void ExceptionTest()
		{
			Assert.AreEqual("phil•an•thropic", hyphenator.HyphenateText("philanthropic"));
		}

		[Test]
		public void ChangeSymbolTest()
		{
			var hyphenator1 = new Hyphenator(HyphenatePatternsLanguage.EnglishUs);
			Assert.AreEqual("dis&shy;ci&shy;plines", hyphenator1.HyphenateText("disciplines"));
		}

		[Test]
		public void HyphenateText()
		{
			var text = @"The arts are a vast subdivision of culture, composed of many creative endeavors and disciplines. It is a broader term than ""art"", which as a description of a field usually means only the visual arts. The arts encompass the visual arts, the literary arts and the performing arts – music, theatre, dance and film, among others. This list is by no means comprehensive, but only meant to introduce the concept of the arts. For all intents and purposes, the history of the arts begins with the history of art. The arts might have origins in early human evolutionary prehistory. According to a recent suggestion, several forms of audio and visual arts (rhythmic singing and drumming on external objects, dancing, body and face painting) were developed very early in hominid evolution by the forces of natural selection in order to reach an altered state of consciousness. In this state, which Jordania calls battle trance, hominids and early human were losing their individuality, and were acquiring a new collective identity, where they were not feeling fear or pain, and were religiously dedicated to the group interests, in total disregards of their individual safety and life. This state was needed to defend early hominids from predators, and also to help to obtain food by aggressive scavenging. Ritualistic actions involving heavy rhythmic music, rhythmic drill, coupled sometimes with dance and body painting had been universally used in traditional cultures before the hunting or military sessions in order to put them in a specific altered state of consciousness and raise the morale of participants.";
			var hyphenateText = new Hyphenator(HyphenatePatternsLanguage.EnglishUs, "-",hyphenateLastWord:true).HyphenateText(text);
			var expected = @"The arts are a vast sub-di-vi-sion of cul-ture, com-posed of many cre-ative endeavors and dis-ci-plines. It is a broader term than ""art"", which as a descrip-tion of a field usu-ally means only the visual arts. The arts encom-pass the visual arts, the lit-erary arts and the per-forming arts – music, the-atre, dance and film, among others. This list is by no means com-pre-hen-sive, but only meant to intro-duce the con-cept of the arts. For all intents and pur-poses, the his-tory of the arts begins with the his-tory of art. The arts might have ori-gins in early human evo-lu-tionary pre-his-tory. According to a recent sugges-tion, sev-eral forms of audio and visual arts (rhythmic singing and drum-ming on external objects, dancing, body and face painting) were devel-oped very early in hominid evo-lu-tion by the forces of nat-ural selec-tion in order to reach an altered state of con-scious-ness. In this state, which Jor-dania calls battle trance, hominids and early human were losing their indi-vid-u-ality, and were acquiring a new col-lec-tive iden-tity, where they were not feeling fear or pain, and were reli-giously ded-i-cated to the group inter-ests, in total dis-re-gards of their indi-vidual safety and life. This state was needed to defend early hominids from preda-tors, and also to help to obtain food by aggres-sive scavenging. Rit-u-al-istic actions involving heavy rhythmic music, rhythmic drill, cou-pled some-times with dance and body painting had been uni-ver-sally used in tra-di-tional cul-tures before the hunting or mil-i-tary ses-sions in order to put them in a spe-cific altered state of con-scious-ness and raise the morale of par-tic-i-pants.";
			Assert.AreEqual(expected, hyphenateText);
		}

		[Test]
		public void HyphenateLastWordOptionFalseTest()
		{
			var text = @".. consciousness and raise the morale of participants.";
			var expected = @".. con-scious-ness and raise the morale of participants.";
			var hyphenateText = new Hyphenator(HyphenatePatternsLanguage.EnglishUs, "-", hyphenateLastWord: false).HyphenateText(text);
			Assert.AreEqual(expected, hyphenateText);
		}		
		
		[Test]
		public void HyphenateLastWordOptionTrueTest()
		{
			var text = @".. consciousness and raise the morale of participants.";
			var expected = @".. con-scious-ness and raise the morale of par-tic-i-pants.";
			var hyphenateText = new Hyphenator(HyphenatePatternsLanguage.EnglishUs, "-", hyphenateLastWord: true).HyphenateText(text);
			Assert.AreEqual(expected, hyphenateText);
		}		
		
		[Test]
		public void MinLetterCountTest()
		{
			var text = @"disciplines";
			var expected = @"disci-plines";
			var hyphenateText = new Hyphenator(HyphenatePatternsLanguage.EnglishUs, "-", minLetterCount: 4).HyphenateText(text);
			Assert.AreEqual(expected, hyphenateText);
		}		
		
		[Test]
		public void MinLetterCountDontRaiseException()
		{
			var text = @"disciplines";
			var expected = @"disciplines";
			var hyphenateText = new Hyphenator(HyphenatePatternsLanguage.EnglishUs, "-", minLetterCount: 50).HyphenateText(text);
			Assert.AreEqual(expected, hyphenateText);
		}			
		
		[Test]
		public void MinLetterCountDontRaiseExceptionIfNegativeValue()
		{
			var text = @"disciplines";
			var expected = @"dis-ci-plines";
			var hyphenateText = new Hyphenator(HyphenatePatternsLanguage.EnglishUs, "-", minLetterCount: -1).HyphenateText(text);
			Assert.AreEqual(expected, hyphenateText);
		}		
		
		[Test]
		public void MinWordLengthTest()
		{
			var text = @"disciplines";
			var expected = @"disciplines";
			var hyphenateText = new Hyphenator(HyphenatePatternsLanguage.EnglishUs, "-", minWordLength: 50).HyphenateText(text);
			Assert.AreEqual(expected, hyphenateText);
		}		
		
		[Test]
		public void MinWordLengthDontRaiseExceptionTest()
		{
			var text = @"disciplines";
			var expected = @"dis-ci-plines";
			var hyphenateText = new Hyphenator(HyphenatePatternsLanguage.EnglishUs, "-", minWordLength:-50).HyphenateText(text);
			Assert.AreEqual(expected, hyphenateText);
		}		
        
        
        
        [Test]
		public void CrashOnExceptionWord()
		{
			var text = @"сегодня";
            var expected = @"се-го-дня";
			var hyphenateText = new Hyphenator(HyphenatePatternsLanguage.Russian, "-", -50).HyphenateText(text);
			Assert.AreEqual(expected, hyphenateText);
		}
	}
}