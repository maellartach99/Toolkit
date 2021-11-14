using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Pokemon.Data
{
	public class Form
	{
		private static Dictionary<string,Dictionary<string,Form>> dictionary;
		public static string[] Alternative(string species)
		{
			try { return dictionary[species].Keys.ToArray(); }
			catch(KeyNotFoundException) { }
			return new string[] { };
		}
		public static Form Alternative(string species,string form)
		{
			if (dictionary[species].ContainsKey(form))
				return dictionary[species][form];
			return null;
		}

		[JsonIgnore] public string ID { get; private set; }
		[JsonIgnore] public Species Species { get => (Species)species; }

		[JsonIgnore] public Item MegaStone { get => (Item)megaItem; }
		[JsonIgnore] public Move MegaMove { get => (Move)megaMove; }

		[JsonIgnore] public Type[] Types
		{
			get
			{
				if (types != null) return types.Select(t => (Type)t).ToArray();
				else return Species.Base.Types;
			}
		}
		[JsonIgnore] public Dictionary<Stat, byte> BaseStats
		{
			get 
			{
				if (stats != null)
					return stats.ToDictionary(p => (Stat)p.Key, p => p.Value);
				else return Species.Base.BaseStats;
			}
		}
		[JsonIgnore] public Dictionary<Stat, byte> Effort
		{
			get
			{
				if (effort != null)
					return effort.ToDictionary(p => (Stat)p.Key, p => p.Value);
				else return Species.Base.Effort;
			}
		}
		[JsonIgnore] public Dictionary<string, Ability> Abilities
		{
			get
			{
				if (abilities != null)
					return abilities.ToDictionary(p => p.Key, p => (Ability)p.Value);
				else return Species.Base.Abilities;
			}
		}
		[JsonIgnore] public Dictionary<string, Item> HoldItems
		{
			get
			{
				if (holdItems != null)
					return holdItems.ToDictionary(p => p.Key, p => (Item)p.Value);
				else return Species.Base.HoldItems;
			}
		}

		[JsonIgnore] public double Height { get => (height == null) ? Species.Base.Height : (double)height; }
		[JsonIgnore] public double Weight { get => (weight == null) ? Species.Base.Weight : (double)weight; }
		[JsonIgnore] public string Kind { get => (kind == null) ? Species.Base.Kind : kind; }
		[JsonIgnore] public string Pokedex { get => (pokedex == null) ? Species.Base.Pokedex : pokedex; }

		[JsonIgnore] public Learn Pool
		{
			get
			{
				Learn result = (Learn)(species + (ID == "" ? ID : "_" + ID));
				if (result == null) return Species.Base.Pool;
				return result;
			}
		}
		[JsonIgnore] public Evolution[] Evolutions
		{
			get
			{
				if (evolutions != null)
					return (Evolution[])evolutions.Clone();
				else return (Evolution[])Species.Base.evolutions.Clone();
			}
		}

		public Form()
		{
			ID = "";
		}

		static Form()
		{
			string json = Read.Data("form");
			dictionary = JsonConvert.DeserializeObject<Dictionary<string,Dictionary<string,Form>>>(json);
			
			foreach (var especie in dictionary)
			{
				foreach (var forma in especie.Value)
				{
					forma.Value.ID = forma.Key;
					forma.Value.species = especie.Key;
					if (!forma.Value.test())
						throw new Exception(especie.Key + " " + forma.Key);
				}
			}
		}

		internal bool test()
		{
			if (ID == "")
			{
				if (types == null) return false;
				if (stats == null) return false;
				if (effort == null) return false;
				if (abilities == null) return false;
				if (holdItems == null) return false;
				if (height == null) return false;
				if (weight == null) return false;
				if (kind == null) return false;
				if (pokedex == null) return false;
				if (evolutions == null) return false;
			}
			
			if (types != null)
				foreach (string type in types)
					if (!Type.Names.Contains(type))
						return false;
			if (abilities != null)
				foreach (var ability in abilities)
					if (!Ability.Names.Contains(ability.Value))
						return false;
			if (holdItems != null)
				foreach (var item in holdItems)
					if (!Item.Names.Contains(item.Value))
						return false;
			if (evolutions != null)
				foreach (var evo in evolutions)
					if (!evo.test()) return false;

			return true;
		}

		[JsonProperty] internal string species;
		[JsonProperty] private string megaItem;
		[JsonProperty] private string megaMove;
		[JsonProperty] private string[] types;
		[JsonProperty] private Dictionary<string, byte> stats;
		[JsonProperty] private Dictionary<string, byte> effort;
		[JsonProperty] private Dictionary<string, string> abilities;
		[JsonProperty] private Dictionary<string, string> holdItems;
		[JsonProperty] private double? height;
		[JsonProperty] private double? weight;
		[JsonProperty] private string kind;
		[JsonProperty] private string pokedex;
		[JsonProperty] private Evolution[] evolutions;
	}
}
