using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NHyphenator
{
	public class Hypenator
	{
		private const char Marker = '.';
		private readonly bool hypenateLastWord;
		private readonly string hyphenateSymbol;
		private readonly int minLetterCount;
		private readonly int minWordLength;
		private Dictionary<string, int[]> exceptions = new Dictionary<string, int[]>();
		private List<Pattern> patterns;

		/// <summary>
		/// Implementation of Frank Liang's hyphenation algorithm
		/// </summary>
		/// <param name="language">Language for load hyphenation patterns</param>
		/// <param name="hyphenateSymbol">Symbol used for denote hyphenation</param>
		/// <param name="minWordLength">Minimum word length for hyphenation word</param>
		/// <param name="minLetterCount">Minimum number of characters left on line</param>
		/// <param name="hypenateLastWord">Hyphenate last word, NOTE: this option work only if input text contains not one word</param>
		public Hypenator(HypenatePatternsLanguage language, string hyphenateSymbol = "&shy;", int minWordLength = 5, int minLetterCount = 3, bool hypenateLastWord = false)
		{
			this.hyphenateSymbol = hyphenateSymbol;
			this.minWordLength = minWordLength;
			this.minLetterCount = minLetterCount;
			this.hypenateLastWord = hypenateLastWord;
			LoadPatterns(language);
		}

		private void LoadPatterns(HypenatePatternsLanguage language)
		{
			//Used TEX hyphenation patterns. Read more on http://tug.org/tex-hyphen/

			switch (language)
			{
				case HypenatePatternsLanguage.EnglishUs:
					CreatePatterns(Patterns.hyph_en_us_pat, Patterns.hyph_en_us_hyp);
					break;
				case HypenatePatternsLanguage.EnglishBritish:
					CreatePatterns(Patterns.hyph_en_gb_pat, Patterns.hyph_en_us_hyp);
					break;
				case HypenatePatternsLanguage.Russian:
					CreatePatterns(Patterns.hyph_ru_pat, Patterns.hyph_ru_hyp);
					break;
				default:
					throw new ArgumentOutOfRangeException("language");
			}
		}

		private void CreatePatterns(string patternsString, string exeptionsString)
		{
			var sep = new[] {' ', '\n', '\r'};
			patterns = patternsString.Split(sep, StringSplitOptions.RemoveEmptyEntries).Select(CreatePattern).ToList();
			exceptions = exeptionsString.Split(sep, StringSplitOptions.RemoveEmptyEntries).ToDictionary(x => x.Replace("-", ""), CreateHyphenateMaskFromExceptionString);
		}


		public string HyphenateText(string text)
		{
			var currentWord = new StringBuilder();
			var result = new StringBuilder();
			if (hypenateLastWord == false)
			{
				string lastWord = FindLastWord(text);
				if (lastWord.Length > 0)
					text = text.Remove(text.Length - lastWord.Length);

				foreach (char c in text)
				{
					if (char.IsLetter(c))
						currentWord.Append(c);
					else
					{
						if (currentWord.Length > 0)
						{
							result.Append(HyphenateWord(currentWord.ToString()));
							currentWord.Clear();
						}
						result.Append(c);
					}
				}

				return result.Append(HyphenateWord(currentWord.ToString())).Append(lastWord).ToString();
			}


			foreach (char c in text)
			{
				if (char.IsLetter(c))
					currentWord.Append(c);
				else
				{
					if (currentWord.Length > 0)
					{
						result.Append(HyphenateWord(currentWord.ToString()));
						currentWord.Clear();
					}
					result.Append(c);
				}
			}

			return result.Append(HyphenateWord(currentWord.ToString())).ToString();
		}

		private string FindLastWord(string phrase)
		{
			var currentWord = new StringBuilder();
			for (int i = phrase.Length - 1; i >= 0; i--)
			{
				if (char.IsLetter(phrase[i]))
					currentWord.Append(phrase[i]);
				else if (currentWord.Length > 0 && currentWord.ToString().Any(char.IsLetter))
					return new string(currentWord.ToString().Reverse().ToArray());
				else
					currentWord.Append(phrase[i]);
			}

			return string.Empty;
		}

		private string HyphenateWord(string originalWord)
		{
			if (ValidForHypenate(originalWord))
				return originalWord;

			string word = originalWord.ToLowerInvariant();
			int[] hyphenationMask;
			if (exceptions.ContainsKey(word))
				hyphenationMask = exceptions[word];
			else
			{
				int[] levels = GenerateLevelsForWord(word);
				hyphenationMask = CreateHyphenateMaskFromLevels(levels);
				CorrectMask(hyphenationMask);
			}
			return HyphenateByMask(originalWord, hyphenationMask);
		}

		private void CorrectMask(int[] hyphenationMask)
		{
			Array.Clear(hyphenationMask, 0, minLetterCount);
			Array.Clear(hyphenationMask, hyphenationMask.Length - 3, minLetterCount);
		}

		private bool ValidForHypenate(string originalWord)
		{
			return originalWord.Length <= minWordLength;
		}

		private int[] GenerateLevelsForWord(string word)
		{
			string wordString = new StringBuilder().Append(Marker).Append(word).Append(Marker).ToString();
			var levels = new int[wordString.Length];
			for (int i = 0; i < wordString.Length - 2; ++i)
			{
				int patternIndex = 0;
				for (int count = 1; count <= wordString.Length - i; ++count)
				{
					var patternFromWord = new Pattern(wordString.Substring(i, count));
					if (Pattern.Compare(patternFromWord, patterns[patternIndex]) < 0)
						continue;
					patternIndex = patterns.FindIndex(patternIndex, pattern => Pattern.Compare(pattern, patternFromWord) > 0);
					if (patternIndex == -1)
						break;
					if (Pattern.Compare(patternFromWord, patterns[patternIndex]) >= 0)
						for (int levelIndex = 0; levelIndex < patterns[patternIndex].GetLevelsCount() - 1; ++levelIndex)
						{
							int level = patterns[patternIndex].GetLevelByIndex(levelIndex);
							if (level > levels[i + levelIndex])
								levels[i + levelIndex] = level;
						}
				}
			}
			return levels;
		}

		private static int[] CreateHyphenateMaskFromLevels(int[] levels)
		{
			int length = levels.Length - 2;
			var hyphenationMask = new int[length];
			for (int i = 0; i < length; i++)
			{
				if (i != 0 && levels[i + 1]%2 != 0)
					hyphenationMask[i] = 1;
				else
					hyphenationMask[i] = 0;
			}
			return hyphenationMask;
		}

		private string HyphenateByMask(string originalWord, int[] hyphenationMask)
		{
			var result = new StringBuilder();
			for (int i = 0; i < originalWord.Length; i++)
			{
				if (hyphenationMask[i] > 0)
					result.Append(hyphenateSymbol);
				result.Append(originalWord[i]);
			}
			return result.ToString();
		}

		private int[] CreateHyphenateMaskFromExceptionString(string s)
		{
			int[] array = Regex.Split(s, @"[a-z]", RegexOptions.Compiled).Select(c => c == "-" ? 1 : 0).ToArray();
			return array;
		}

		private Pattern CreatePattern(string pattern)
		{
			var levels = new List<int>(pattern.Length);
			var resultStr = new StringBuilder();
			bool waitDigit = true;
			foreach (char c in pattern)
			{
				if (Char.IsDigit(c))
				{
					levels.Add(Int32.Parse(c.ToString(CultureInfo.InvariantCulture)));
					waitDigit = false;
				}
				else
				{
					if (waitDigit)
						levels.Add(0);
					resultStr.Append(c);
					waitDigit = true;
				}
			}

			if (waitDigit)
				levels.Add(0);

			return new Pattern(resultStr.ToString(), levels);
		}
	}

	public enum HypenatePatternsLanguage
	{
		EnglishUs,
		EnglishBritish,
		Russian
	}
}