using System;
using System.Collections.Generic;
using System.Linq;

namespace NHyphenator
{
	public class Pattern : IComparer<Pattern>, IComparable<Pattern>
	{
		private readonly string str;

		public int[] Levels { get; set; }

		public Pattern(string str, IEnumerable<int> levels)
		{
			this.str = str;
			Levels = levels.ToArray();
		}


		public Pattern(string str)
		{
			this.str = str;
			Levels = new int[0];
		}

		public static int Compare(Pattern x, Pattern y)
		{
			bool first = x.str.Length < y.str.Length;
			int minSize = first ? x.str.Length : y.str.Length;
			for (var i = 0; i < minSize; ++i)
			{
				if (x.str[i] < y.str[i])
					return -1;
				if (x.str[i] > y.str[i])
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