using System;
using System.Collections.Generic;

namespace NHyphenator
{
	public class Pattern : IComparer<Pattern>, IComparable<Pattern>
	{
		public string Str { get; set; }
		public List<int> Levels { get; set; }

		public Pattern()
		{
			Levels = new List<int>();
		}

		public static int Compare(Pattern x, Pattern y)
		{
			bool first = x.Str.Length < y.Str.Length;
			int minSize = first ? x.Str.Length : y.Str.Length;
			for (var i = 0; i < minSize; ++i)
			{
				if (x.Str[i] < y.Str[i])
					return -1;
				if (x.Str[i] > y.Str[i])
					return 1;
			}
			return first ? -1 : 1;
		}

		int IComparer<Pattern>.Compare(Pattern x, Pattern y)
		{
			return Compare(x, y);
		}

		public int CompareTo(Pattern other)
		{
			return Compare(this, other);
		}
	}
}