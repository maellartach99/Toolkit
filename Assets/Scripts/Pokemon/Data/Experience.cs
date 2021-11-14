using System;
using System.Collections.Generic;

namespace Pokemon.Data
{
	public class Experience
	{
		public delegate int Table(int level);
		public static readonly int MaxLevel = 100;
		public int MaxExp { get => GetExp(MaxLevel); }

		public int GetExp(int level)
		{
			level = Range(0, level, MaxLevel);
			if (level <= 0) return -1;
			return _function(level);
		}

		public int GetLevel(int exp)
		{
			exp = Range(0, exp, MaxExp);
			for (int i = 0; i <= MaxLevel; i++)
			{
				int x = GetExp(i);
				if (exp == x) return i;
				if (exp < x) return i - 1;
			}
			return MaxLevel;
		}

		public Experience(string fun)
			=> _function = functions[fun];

		static Experience()
		{
			functions = new Dictionary<string, Table>();
			functions["medium"]      = delegate (int level) { return (int)Math.Pow(level, 3); };
			functions["erratic"]     = delegate (int level) { return 6 * (int)Math.Pow(level, 4) / 1000; };
			functions["fluctuating"] = delegate (int level) { return Math.Max(40, 132 - level/2) * (int)Math.Pow(level, 4) / 5000; };
			functions["parabolic"]   = delegate (int level) { return 6 * (int)Math.Pow(level, 3) / 5 - 15 * (int)Math.Pow(level, 2) + 100 * level - 140; };
			functions["fast"]        = delegate (int level) { return 4 * (int)Math.Pow(level, 3) / 5; };
			functions["slow"]        = delegate (int level) { return 5 * (int)Math.Pow(level, 3) / 4; };
		}

		static int Range(int min,int val,int max)
			=> Math.Max(min, Math.Min(val, max));

		private Table _function;
		private static Dictionary<string, Table> functions;
	}
}
