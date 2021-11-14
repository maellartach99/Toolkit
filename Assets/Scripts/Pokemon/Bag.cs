using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon.Data;
using Newtonsoft.Json;

namespace Pokemon
{
	[JsonObject]
	public class Pocket
	{
		[JsonProperty]
		public string Name { get; private set; }
		[JsonProperty]
		public bool AutoSort { get; private set; }
		[JsonProperty]
		public int MaxSlot { get; private set; }
		[JsonProperty]
		public bool CanGive { get; private set; }
		[JsonIgnore]
		public int Count { get => items.Count; }
		[JsonIgnore]
		public string[] List { get => keys.ToArray(); }

		public Pocket(string name, bool autoSort = false, int maxSlot = 99, bool canGive = true)
		{
			Name = name;
			AutoSort = autoSort;
			MaxSlot = maxSlot;
			CanGive = canGive;
			items = new Dictionary<string, uint>();
			keys = new List<string>();
		}

		public void Swap(int a, int b)
		{
			a = (a % Count + Count) % Count;
			b = (b % Count + Count) % Count;
			(keys[a], keys[b]) = (keys[b], keys[a]);
		}

		public uint this[Item item]
		{
			get
			{
				if (items.ContainsKey(item.ID))
					return items[item.ID];
				return 0;
			}
		}

		public bool Add(Item item, uint num = 1)
		{
			if (items.ContainsKey(item.ID))
			{
				if (items[item.ID] + num > MaxSlot)
					return false;
				items[item.ID] += num;
			}
			else
			{
				if (num > MaxSlot) return false;
				keys.Add(item.ID);
				items[item.ID] = num;
			}
			return true;
		}

		public bool Remove(Item item, uint num = 1)
		{
			if (items.ContainsKey(item.ID))
			{
				if (items[item.ID] < num) return false;
				if (items[item.ID] == num)
				{
					items.Remove(item.ID);
					keys.Remove(item.ID);
				}
				else items[item.ID] -= num;
				return true;
			}
			return false;
		}

		[JsonProperty]
		private Dictionary<string, uint> items;
		[JsonProperty]
		private List<string> keys;
	}

	[JsonObject]
	public class Bag
	{
		public Pocket this[string name] { get => dict[name]; }
		public uint this[Item item] { get => dict[item.Pocket][item]; }

		public Bag()
		{
			dict = new Dictionary<string, Pocket>()
			{
				{"items",    new Pocket("Objetos")},
				{"medicines",new Pocket("Botiquín")},
				{"pokeballs",new Pocket("Poké Balls")},
				{"tms",      new Pocket("MTs y MOs", true,1,false)},
				{"berries",  new Pocket("Bayas")},
				{"battle",   new Pocket("Objetos de Batalla")},
				{"key",      new Pocket("Objetos Clave",maxSlot:1,canGive:false)},
			};
		}

		public bool Add(Item item, uint num = 1)
			=> dict[item.Pocket].Add(item, num);
		public bool Remove(Item item, uint num = 1)
			=> dict[item.Pocket].Remove(item, num);
		
		[JsonProperty]
		private Dictionary<string, Pocket> dict;
		[JsonIgnore]
		public string[] Pockets { get => dict.Keys.ToArray();}
	}
}
