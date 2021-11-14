using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pokemon.Data.Exceptions;

namespace Pokemon.Data
{
	[JsonObject]
	public class Ability
	{
		private static Dictionary<string, Ability> dictionary;
		public static string[] Names { get => new List<string>(dictionary.Keys).ToArray(); }

		public static implicit operator string(Ability s) { return s.ID; }
		public static implicit operator Ability(string s) { return dictionary[s]; }

		public string ID { get; private set; }
		[JsonIgnore] public int Num { get => num; }
		[JsonIgnore] public double Rating { get => rating; }
		[JsonIgnore] public string Name { get => name; }
		[JsonIgnore] public string Description { get => description; }

        #region Private

        [JsonConstructor] private Ability() { }

		static Ability()
		{
			string json = Read.Data("ability");
			dictionary = JsonConvert.DeserializeObject< Dictionary<string,Ability> >(json);

			foreach (var pair in dictionary)
				pair.Value.ID = pair.Key;
		}

		[JsonProperty] private int num;
		[JsonProperty] private double rating;
		[JsonProperty] private string name;
		[JsonProperty] private string description;

        #endregion
    }
}
