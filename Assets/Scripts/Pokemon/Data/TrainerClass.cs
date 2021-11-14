using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Pokemon.Data
{
	public class TrainerClass
	{
		private static Dictionary<string, TrainerClass> dictionary;
		public static string[] Names { get { return new List<string>(dictionary.Keys).ToArray(); } }

		public static explicit operator string(TrainerClass s) { return s.ID; }
		public static explicit operator TrainerClass(string s) { return dictionary[s]; }

		public string ID { get; private set; }
		public string Name { get; private set; }
		public Gender Gender { get; private set; }
		public int Money { get; private set; }
		public int Skill { get; private set; }

		private TrainerClass(string id, Dictionary<string, object> json)
		{
			ID = id;
			Name = (string)json["name"];
			Gender = (Gender)(string)json["gender"];
			Money = Convert.ToInt32(json["money"]);
			try { Skill = Convert.ToInt32(json["skill"]); }
			catch { Skill = Money; }
		}

		static TrainerClass()
		{
			string json = Read.Data("trainer");
			var dict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);
			dictionary = dict.ToDictionary(p => p.Key, p => new TrainerClass(p.Key, p.Value));
		}
	}
}
