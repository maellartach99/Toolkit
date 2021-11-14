using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Pokemon.Data
{
	public class Item
	{
		private static Dictionary<string, Item> dictionary;
		public static string[] Names { get => new List<string>(dictionary.Keys).ToArray(); }

		public static explicit operator string(Item i) { return i.ID; }
		public static explicit operator Item(string s) { return dictionary[s]; }

		public string ID { get; private set; }
		[JsonIgnore] public int Num { get => num; }
		[JsonIgnore] public string Name { get => name; }
		[JsonIgnore] public string PluralName { get => pluralName; }
		[JsonIgnore] public int Price { get => price; }
		[JsonIgnore] public string Pocket { get => pocket; }
		[JsonIgnore] public string Description 
		{
			get 
			{
				if (Teach != null) return Teach.Description;
				else return description;
			}
		}
		[JsonIgnore] public Move Teach { get => (teach != null) ? (Move)teach : null;  }
		[JsonIgnore] public Type Plate { get => (plate != null) ? (Type)plate : null; }
		[JsonIgnore] public Type Gem { get => (gem != null) ? (Type)gem : null; }
		[JsonIgnore] public string Icon
		{
			get
			{
				string result = "Icons/Items/"+Pocket;
				if (Teach != null)
					result += "/tm-" + Teach.Type.ID;
				else result += "/" + ID;
				return result;
			}
		}

		[JsonConstructor] private Item() { }

		static Item()
		{
			string json = Read.Data("item");
			dictionary = JsonConvert.DeserializeObject< Dictionary<string,Item> >(json);

			foreach (var pair in dictionary)
				pair.Value.ID = pair.Key;
		}

		[JsonProperty] private int num;
		[JsonProperty] private string name;
		[JsonProperty] private string pluralName;
		[JsonProperty] private int price;
		[JsonProperty] private string pocket;
		[JsonProperty] private string description;
		[JsonProperty] private string teach;
		[JsonProperty] private string plate;
		[JsonProperty] private string gem;
	}
}
