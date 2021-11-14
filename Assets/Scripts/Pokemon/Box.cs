using System;
using Newtonsoft.Json;

namespace Pokemon
{
	[JsonObject]
	public class Box
	{
		public static readonly int Size = 30;
		[JsonProperty]
		public string Name { get; set; }
		[JsonProperty]
		private Pokemon[] pokemon;

		[JsonIgnore]
		public int Count
		{
			get
			{
				int result = 0;
				for (int i = 0; i < Size; i++)
					if (pokemon[i] != null) result++;
				return result;
			}
		}

		public Pokemon this[int index]
		{
			get { return pokemon[index]; }
			set { pokemon[index] = value; }
		}

		public bool Add(Pokemon pkm)
		{
			if (Count == Size) return false;
			for (int i = 0; i < Size; i++)
				if(pokemon[i] == null)
				{
					pokemon[i] = pkm;
					return true;
				}
			return false;
		}

		public Box(string name)
		{
			Name = name;
			pokemon = new Pokemon[Size];
		}
	}
}
