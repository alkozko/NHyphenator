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
		private readonly string hyphenateSymbol;
		private Dictionary<string, WordHyphenation> exceptions = new Dictionary<string, WordHyphenation>();
		private List<Pattern> patterns;

		public Hypenator(HypenatePatternsLanguage language, string hyphenateSymbol)
		{
			this.hyphenateSymbol = hyphenateSymbol;
			LoadPatterns(language);
		}

		private void LoadPatterns(HypenatePatternsLanguage language)
		{
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
			;
		}


		public string HyphenateText(string text)
		{
			var currentWord = new StringBuilder();
			var result = new StringBuilder();

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

		private string FindLastWord(string phrase)
		{
			var currentWord = new StringBuilder();
			for (int i = phrase.Length - 1; i >= 0; i--)
			{
				if (char.IsLetter(phrase[i]))
					currentWord.Append(phrase[i]);
				else if (currentWord.Length > 0 && currentWord.ToString().Any(char.IsLetter) && currentWord.Length < 10)
					return new string(currentWord.ToString().Reverse().ToArray());
				else
					currentWord.Append(phrase[i]);
			}

			return string.Empty;
		}

		public string HyphenateWord(string originalWord)
		{
			if (originalWord.Length <= 5)
				return originalWord;

			string word = originalWord.ToLowerInvariant();
			WordHyphenation hyphenationMask;
			if (exceptions.ContainsKey(word))
				hyphenationMask = exceptions[word];
			else
			{
				int[] levels = GenerateLevelsFowWord(word);
				hyphenationMask = CreateHyphenateMaskFromLevels(levels);
				hyphenationMask.Mask[0] = 0;
				hyphenationMask.Mask[1] = 0;
				hyphenationMask.Mask[2] = 0;
				hyphenationMask.Mask[hyphenationMask.MaskSize - 1] = 0;
				hyphenationMask.Mask[hyphenationMask.MaskSize - 2] = 0;
				hyphenationMask.Mask[hyphenationMask.MaskSize - 3] = 0;
			}
			return HyphenateByMask(originalWord, hyphenationMask);
		}

		private int[] GenerateLevelsFowWord(string word)
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
						for (int levelIndex = 0; levelIndex < patterns[patternIndex].Levels.Length - 1; ++levelIndex)
						{
							int level = patterns[patternIndex].Levels[levelIndex];
							if (level > levels[i + levelIndex])
								levels[i + levelIndex] = level;
						}
				}
			}
			return levels;
		}

		private static WordHyphenation CreateHyphenateMaskFromLevels(int[] levels)
		{
			var hyphenationMask = new WordHyphenation {MaskSize = levels.Length - 2, Mask = new int[levels.Length - 2]};
			for (int i = 0; i < hyphenationMask.MaskSize; i++)
			{
				if (levels[i + 1]%2 != 0 && i != 0)
					hyphenationMask.Mask[i] = 1;
				else
					hyphenationMask.Mask[i] = 0;
			}
			return hyphenationMask;
		}

		private string HyphenateByMask(string originalWord, WordHyphenation hyphenationMask)
		{
			var result = new StringBuilder();
			for (int i = 0; i < originalWord.Length; i++)
			{
				if (hyphenationMask.Mask[i] > 0)
					result.Append(hyphenateSymbol);
				result.Append(originalWord[i]);
			}
			return result.ToString();
		}

		private WordHyphenation CreateHyphenateMaskFromExceptionString(string s)
		{
			int[] array = Regex.Split(s, @"[a-z]",RegexOptions.Compiled).Select(c => c == "-" ? 1 : 0).ToArray();
			return new WordHyphenation
				{
					Mask = array,
					MaskSize = array.Length
				};
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