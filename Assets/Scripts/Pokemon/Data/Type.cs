using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pokemon.Data.Exceptions;
using UnityEngine;

namespace Pokemon.Data
{
	[JsonObject]
	public class Type
	{
		private static Dictionary<string, Type> dictionary;
		public static string[] Names { get => new List<string>(dictionary.Keys).ToArray(); }

		public string ID { get; private set; }
		[JsonIgnore] public string Name { get => name; }
		[JsonIgnore] public Color Color { get => new Color(color["r"]/255,color["g"]/255,color["b"]/255); }
		[JsonIgnore] public bool Special { get => special; }
		[JsonIgnore] public string Icon { get => "Icons/Types/" + ID;}

		[JsonIgnore]
		public Type[] Weaknesses  { get => weaknesses.Select<string,Type>(x => (Type)x).ToArray();  }
		[JsonIgnore]
		public Type[] Resistances { get => resistances.Select<string,Type>(x => (Type)x).ToArray();  }
		[JsonIgnore]
		public Type[] Immunities  { get => immunities.Select<string,Type>(x => (Type)x).ToArray();  }

		public static explicit operator string(Type t) { return t.ID; }
		public static explicit operator Type(string s) { return dictionary[s]; }

		public double Effectiveness(params Type[] target)
		{
			double result = 1.0;
			foreach (Type t in target)
			{
				if (t.Weaknesses.Contains(this))
					result *= 2;
				if (t.Resistances.Contains(this))
					result /= 2;
				if (t.Immunities.Contains(this))
					return 0;
			}
			return result;
		}

        #region Private

        [JsonConstructor] private Type() { }

		static Type()
		{
			string json = Read.Data("type");
			dictionary = JsonConvert.DeserializeObject< Dictionary<string,Type> >(json);

			foreach (var pair in dictionary)
			{
				pair.Value.ID = pair.Key;
				if (!pair.Value.test()) throw new Exception("Type "+ pair.Key);
			}
		}

		private bool test()
		{
			if (this.name == null) return false;
			if (this.color == null) return false;
			if (this.weaknesses == null) return false;
			if (this.resistances == null) return false;
			if (this.immunities == null) return false;

			foreach (string type in weaknesses)
				if (!dictionary.ContainsKey(type))
					return false;
			foreach (string type in resistances)
				if (!dictionary.ContainsKey(type))
					return false;
			foreach (string type in immunities)
				if (!dictionary.ContainsKey(type))
					return false;

			return true;
		}

		#endregion

		[JsonProperty] private string name;
		[JsonProperty] private Dictionary<string,float> color;
		[JsonProperty] private bool special;
		[JsonProperty] private List<string> weaknesses;
		[JsonProperty] private List<string> resistances;
		[JsonProperty] private List<string> immunities;
	}
}
