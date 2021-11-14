using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Pokemon.Data
{
	[JsonObject]
	public class Nature
	{
		private static Dictionary<string, Nature> dictionary;
		public static string[] Names { get => new List<string>(dictionary.Keys).ToArray(); }

		public static explicit operator string(Nature n) { return n.ID; }
		public static explicit operator Nature(string s) { return dictionary[s]; }

		public string ID { get; private set; }
		[JsonIgnore] public string Name { get => name; }
		[JsonIgnore] public Stat Plus { get => plus; }
		[JsonIgnore] public Stat Minus { get => minus; }
		[JsonIgnore] public bool Neutral { get => (Plus == Minus); }

		public double this[Stat s]
		{
			get
			{
				double result = 1.0;
				if (s == Plus) result *= 1.1;
				if (s == Minus) result /= 1.1;
				return result;
			}
		}

		[JsonConstructor] private Nature() { }

		static Nature()
		{
			string json = Read.Data("nature");
			dictionary = JsonConvert.DeserializeObject< Dictionary<string,Nature> >(json);

			foreach (var pair in dictionary)
				pair.Value.ID = pair.Key;
		}

		[JsonProperty] private string name;
		[JsonProperty] private Stat plus;
		[JsonProperty] private Stat minus;
	}
}
