using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Pokemon.Data
{
	[JsonObject]
	public class Learn
	{
		private static Dictionary<string, Learn> dictionary;
		public static explicit operator Learn(string s)
		{
			try { return dictionary[s]; }
			catch (KeyNotFoundException) { return new Learn(); }
		}

		public Move[] this[int i]
		{
			get 
			{
				if (level.ContainsKey(i))
					return level[i].Select(s => (Move)s).ToArray();
				return new Move[] { };
			}
		}
		[JsonIgnore] public Move[] Egg { get => egg.Select(s => (Move)s).ToArray(); }
		[JsonIgnore] public Move[] Tutor { get => tutor.Select(s => (Move)s).ToArray(); }

		public Learn()
		{
			level = new Dictionary<int, string[]>();
			egg = new string[] { };
			tutor = new string[] { };
		}

		static Learn()
		{
			string json = Read.Data("learn");
			dictionary = JsonConvert.DeserializeObject< Dictionary<string,Learn> >(json);

			foreach (var pair in dictionary)
				if (!pair.Value.test())
					throw new Exception("Learn "+pair.Key);
			
			foreach (string species in Species.Names)
				if (!dictionary.ContainsKey(species))
					throw new Exception("Missing learn for "+species);
		}

		private bool test()
		{
			foreach (var move in egg)
				if (!Move.Names.Contains(move))
					return false;
			foreach (var move in tutor)
				if (!Move.Names.Contains(move))
					return false;
			foreach (var lv in level)
				foreach (var move in lv.Value)
					if (!Move.Names.Contains(move))
						return false;
			
			return true;
		}

		[JsonProperty] private Dictionary<int, string[]> level;
		[JsonProperty] private string[] egg;
		[JsonProperty] private string[] tutor;
	}
}
