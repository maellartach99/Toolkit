using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pokemon.Data
{
	public class Evolution
	{
		[JsonIgnore] public Species ToSpecies { get => (Species)species; }
		[JsonIgnore] public Method Method { get => (Method)Enum.Parse(typeof(Method), method, true); }
		[JsonIgnore] public byte Level { get => level; }
		[JsonIgnore] public Item HasItem { get => (Item)item; }
		[JsonIgnore] public Move HasMove { get => (Move)move; }
		[JsonIgnore] public byte Happiness { get => happiness; }
		[JsonIgnore] public byte Beauty { get => beauty; }
		[JsonIgnore] public Gender Gender { get => (Gender)gender; }
		[JsonIgnore] public Hour Hour { get => (Hour)hour; }
		//[JsonIgnore] public Weather Weather { get => (Weather)weather; }
		[JsonIgnore] public byte? Random { get; private set; }
		[JsonIgnore] public string Location { get => Location; }
		[JsonIgnore] public Type HasMoveOfType { get => (Type)moveType; }
		[JsonIgnore] public Type PartnerType { get => (Type)partyType; }
		[JsonIgnore] public Species PartnerSpecies { get => (Species)partySpecies; }
		[JsonIgnore] public bool Duplicate { get => duplicate; }

		[JsonConstructor] private Evolution() { }

		internal bool test()
		{
			if (species == null) return false;
			if (method == null) return false;
			return true;
		}

		[JsonProperty] private string species;
		[JsonProperty] private string method;
		[JsonProperty] private byte level;
		[JsonProperty] private string item;
		[JsonProperty] private string move;
		[JsonProperty] private byte happiness;
		[JsonProperty] private byte beauty;
		[JsonProperty] private string gender;
		[JsonProperty] private string hour;
		[JsonProperty] private string weather;
		[JsonProperty] private byte? random;
		[JsonProperty] private string location;
		[JsonProperty] private string moveType;
		[JsonProperty] private string partyType;
		[JsonProperty] private string partySpecies;
		[JsonProperty] private bool duplicate;
	}

	public class Hour
	{
		public readonly static Hour Day = new Hour();
		public readonly static Hour Night = new Hour();
		public static Hour Now()
		{
			int h = DateTime.Now.Hour;
			return (h < 8 || h >= 20) ? Night : Day;
		}
		public static explicit operator Hour(string s)
		{
			if (s.ToLower() == "day") return Day;
			if (s.ToLower() == "night") return Night;
			throw new KeyNotFoundException(s);
		}

		private Hour() { }
	}

	public class Gender
	{
		public readonly static Gender Male = new Gender();
		public readonly static Gender Female = new Gender();
		public readonly static Gender None = new Gender();

		public static explicit operator Gender(string s)
		{
			if (s.ToLower()[0] == 'm') return Male;
			if (s.ToLower()[0] == 'f') return Female;
			if (s.ToLower()[0] == 'n') return None;
			return null;
		}

		public override string ToString()
		{
			if (this == Male) return "♂";
			if (this == Female) return "♀";
			return " ";
		}

		private Gender() { }
	}

	public enum Method
	{
		Level, Item, Trade
	}
}
