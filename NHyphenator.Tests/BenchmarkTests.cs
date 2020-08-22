﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace NHyphenator.Tests
{
	public class BenchmarkTests
	{
		private readonly ITestOutputHelper _testOutputHelper;

		public BenchmarkTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		[Fact(Skip = "For manual run")]
		public void Test()
		{
			var text = @"The arts are a vast subdivision of culture, composed of many creative endeavors and disciplines. It is a broader term than ""art"", which as a description of a field usually means only the visual arts. The arts encompass the visual arts, the literary arts and the performing arts – music, theatre, dance and film, among others. This list is by no means comprehensive, but only meant to introduce the concept of the arts. For all intents and purposes, the history of the arts begins with the history of art. The arts might have origins in early human evolutionary prehistory. According to a recent suggestion, several forms of audio and visual arts (rhythmic singing and drumming on external objects, dancing, body and face painting) were developed very early in hominid evolution by the forces of natural selection in order to reach an altered state of consciousness. In this state, which Jordania calls battle trance, hominids and early human were losing their individuality, and were acquiring a new collective identity, where they were not feeling fear or pain, and were religiously dedicated to the group interests, in total disregards of their individual safety and life. This state was needed to defend early hominids from predators, and also to help to obtain food by aggressive scavenging. Ritualistic actions involving heavy rhythmic music, rhythmic drill, coupled sometimes with dance and body painting had been universally used in traditional cultures before the hunting or military sessions in order to put them in a specific altered state of consciousness and raise the morale of participants.";
			var hyphenator = new Hyphenator(HyphenatePatternsLanguage.EnglishUs, "-");
			var stopWatches = new List<long>();

			for (int i = 0; i < 200; i++)
			{
				var	startNew = Stopwatch.StartNew();
				hyphenator.HyphenateText(text);
				startNew.Stop();

				if (i > 10)
					stopWatches.Add(startNew.ElapsedMilliseconds);
			}

			var avg = stopWatches.Average();
			var disp = stopWatches.Select(x => Math.Abs(avg - x)).Max();
			_testOutputHelper.WriteLine(@"{0} ± {1}",avg,disp);
		}		
		
		[Fact(Skip = "For manual run")]
		public void TestWord()
		{
			var hyphenator = new Hyphenator(HyphenatePatternsLanguage.EnglishUs, "-");
			var stopWatches = new List<long>();

			for (int i = 0; i < 1000; i++)
			{
				var	startNew = Stopwatch.StartNew();
				hyphenator.HyphenateText("subdivision");
				hyphenator.HyphenateText("creative");
				hyphenator.HyphenateText("disciplines");
				startNew.Stop();

				if (i > 2)
					stopWatches.Add(startNew.ElapsedMilliseconds);
			}

			var avg = stopWatches.Average();
			var disp = stopWatches.Select(x => Math.Abs(avg - x)).Max();
			_testOutputHelper.WriteLine("{0} ± {1}", avg, disp);
		}
	}
}