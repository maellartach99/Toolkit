using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon.Data
{
	public class Stat
	{
		static Stat()
		{
			list = new Stat[8];
			list[0] = new Stat("hp", 0);
			list[1] = new Stat("atk", 1, "Picante");
			list[2] = new Stat("def", 2, "Ácido");
			list[3] = new Stat("speed", 3, "Dulce");
			list[4] = new Stat("satk", 4, "Seco");
			list[5] = new Stat("sdef", 5, "Amargo");
			list[6] = new Stat("acc", 6);
			list[7] = new Stat("eva", 7);
		}

		public static implicit operator int(Stat s) { return s.Num; }
		public static implicit operator string(Stat s) { return s.Name; }

		public static implicit operator Stat(int i) { return list[i%8]; }
		public static implicit operator Stat(string s)
		{
			foreach (var stat in list)
				if (stat.Name == s) return stat;
			return null;
		}

		private static Stat[] list;

		private Stat(string s, int i, string f = "")
		{
			Name = s;
			Num = i;
			Flavor = f;
		}
		private string Name;
		private int Num;
		public string Flavor { get; private set; }
	}

	public class Stats : IEnumerable<KeyValuePair<Stat,int>>
	{
		public Stats(Stat[] s, int local = -1, int global = -1)
		{
			localMax = local;
			globalMax = global;

			_stats = new Dictionary<Stat,int>();
			foreach (var stat in s)
				_stats[stat] = 0;
		}

		public int this[Stat s]
		{
			get => _stats[s];
			set
			{
				if (!_stats.ContainsKey(s))
					return;
				if (value < 0)
					_stats[s] = 0;
				else if (localMax > 0 && value > localMax)
					_stats[s] = localMax;
				else if (globalMax > 0 && sum(s) + value > globalMax)
					_stats[s] = globalMax - sum(s);
				else _stats[s] = value;
			}
		}

		public int sum(Stat exclude)
		{
			int result = 0;
			foreach (var pair in _stats)
			{
				if (pair.Key != exclude)
					result += pair.Value;
			}
			return result;
		}

		public IEnumerator<KeyValuePair<Stat, int>> GetEnumerator()
			=> _stats.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
			=> ((IEnumerable)_stats).GetEnumerator();

		private Dictionary<Stat,int> _stats;
		private int localMax;
		private int globalMax;
	}
}
