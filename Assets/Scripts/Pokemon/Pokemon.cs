using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pokemon.Data;

namespace Pokemon
{
	[JsonObject]
	public class Pokemon
	{
		public static Random R { get; private set; }
		public static int Lim(int min, int val, int max)
			=> Math.Max(min, Math.Min(val, max));
		public readonly static (int, int) Shinies = (1, 1024);

		static Pokemon() { R = new Random(); }

		[JsonIgnore]
		public int PublicID { get { return id & 0xFFFF; } }
		[JsonIgnore]
		public int SecretID { get { return id >> 16; } }
		[JsonIgnore]
		public Species Species 
		{
			get { return (Species)_species; } 
			set { _species = value.ID; }
		}
		[JsonIgnore]
		public Form Form
		{
			get { return Species[_form]; }
			set { _form = value.ID; } // TODO
		}
		[JsonIgnore]
		public string Name
		{
			get { return (_name == null) ? Species.Name : _name; }
			set { _name = value; }
		}
		[JsonIgnore]
		public Gender Gender
		{
			get
			{
				double gr = Species.GenderRate;
				if (gr < 0) return Gender.None;
				if (_gender < gr) return Gender.Male;
				return Gender.Female;
			}
			set
			{
				double gr = Species.GenderRate;
				if (gr < 0) return;

				if (value == null)
					_gender = R.NextDouble();
				else if (value == Gender.Male)
					_gender = Math.Min(0.0, gr);
				else if (value == Gender.Female)
					_gender = Math.Max(gr, 1.0) - Double.Epsilon;
			}
		}
		[JsonIgnore]
		public bool? Shiny
		{
			get { return _shiny < Shinies.Item1; }
			set 
			{
				if (value == null) _shiny = R.Next(Shinies.Item2);
				else if (value == true) _shiny = Shinies.Item1 - 1;
				else _shiny = Shinies.Item1 + 1;
			}
		}
		[JsonIgnore]
		public Dictionary<Stat,byte> IV
		{
			get
			{
				var result = new Dictionary<Stat, byte>();
				for (int i = 0; i < 6; i++)
					result[i] = _iv[i];
				return result;
			}
			set
			{
				_iv = new byte[6];
				foreach (var pair in value)
					_iv[pair.Key] = Math.Min(pair.Value, (byte)31);
			}
		}
		[JsonIgnore]
		public Dictionary<Stat, byte> EV
		{
			get
			{
				var result = new Dictionary<Stat, byte>();
				for (int i = 0; i < 6; i++)
					result[i] = _ev[i];
				return result;
			}
			set
			{
				_ev = new byte[6];
				int sum = 0;
				foreach (var pair in value)
				{
					sum += pair.Value;
					if (sum > 512) break;
					_ev[pair.Key] = pair.Value;
				}
			}
		}
		[JsonIgnore]
		public Nature Nature
		{
			get { return (Nature)_nature; }
			set { _nature = value.ID; }
		}
		[JsonIgnore]
		public Ability Ability
		{
			get { return Form.Abilities[_ability]; }
			set
			{
				if (value == null)
					_ability = Form.Abilities.Keys.ToArray()[R.Next(Form.Abilities.Count - 1)];
				foreach (var pair in Form.Abilities)
					if (pair.Value == value) _ability = pair.Key;
			}
		}
		[JsonIgnore]
		public int Experience
		{
			get { return _experience; }
			set { _experience = Lim(0,value,Species.Experience.MaxExp); }
		}
		[JsonIgnore]
		public int Level
		{
			get { return Species.Experience.GetLevel(Experience); }
			set { Experience = Species.Experience.GetExp(value); }
		}
		[JsonIgnore]
		public int ToNextLevel
		{
			get { return Species.Experience.GetExp(Level+1) - Experience; }
		}
		[JsonIgnore]
		public Item Item
		{
			get { return (_item == null) ? null : (Item)_item; }
			set { _item = (value == null) ? null : (string)value; }
		}
		[JsonIgnore]
		public MoveSlot[] MoveSet { get { return (MoveSlot[])_MoveSet.Clone(); } }
		public void Swap(int a, int b)
		{
			a = Lim(0, a, _MoveSet.Length);
			b = Lim(0, b, _MoveSet.Length);

			var tmp = _MoveSet[a];
			_MoveSet[a] = _MoveSet[b];
			_MoveSet[b] = tmp;
		}
		[JsonIgnore]
		public Dictionary<Stat,int> Stats
		{
			get
			{
				var result = new Dictionary<Stat, int>();
				for (Stat i = "hp"; i < 6; i++)
					result[i] = S(i);
				return result;
			}
		}
		[JsonIgnore]
		public int HP
		{
			get => _HP;
			set => _HP = Lim(0, value, Stats["hp"]);
		}
		public void HealHP() => HP = Stats["hp"];
		public void HealPP()
		{
			foreach (var slot in MoveSet)
				if (slot.Move != null)
					slot.PP = slot.MaxPP;
		}
		public void Heal() { HealHP(); HealPP(); }

		private string Path
		{
			get
			{
				string result = Species.Num.ToString().PadLeft(3,'0');
				if (Species.GenderDiff && Form.ID == "")
				{
					if (Gender == Data.Gender.Male) result += "m";
					else if (Gender == Data.Gender.Female) result += "f";
				}
				result += (Form.ID != "") ? ("_" + Form.ID) : "";
				return result;
			}
		}
		public string Sprite
		{
			get
			{
				string result = "Icons/Pokemon/";
				result += (Shiny.Value ? "shiny" : "normal")+"/front/";
				return result + Path;
			}
		}
		public string Model
		{
			get => "Pokemon/" + Path;
		}

		[JsonConstructor]
		public Pokemon(string _species, byte level, string _form = "")
			: this((Species)_species,level,_form) { }
		public Pokemon(Species sp, byte level, string form = "")
		{
			id = R.Next(256) | R.Next(256) << 8 | R.Next(256) << 16 | R.Next(256) << 24;
			Shiny = null;
			Species = sp;
			Form = Species[form];
			Name = null;
			Gender = null;
			Happiness = Species.Happiness;
			Trainer = null;

			IV = new Dictionary<Stat, byte>()
			{
				{"hp",  (byte)R.Next(32)}, {"atk",  (byte)R.Next(32)},
				{"def", (byte)R.Next(32)}, {"speed",(byte)R.Next(32)},
				{"satk",(byte)R.Next(32)}, {"sdef", (byte)R.Next(32)}
			};
			EV = new Dictionary<Stat, byte>()
			{
				{"hp",   0}, {"atk", 0}, {"def",  0},
				{"speed",0}, {"satk",0}, {"sdef", 0}
			};
			Nature = (Nature) Nature.Names[R.Next(25)];
			Ability = null;
			Level = level;
			MeetLevel = level;
			Item = HoldItem();
			TimeReceived = DateTime.Now;

			_MoveSet = new MoveSlot[]
			{ 
				new MoveSlot(), new MoveSlot(),
				new MoveSlot(), new MoveSlot()
			};
			int slot = 0;
			for (int l = level; l >= 0; l--)
				foreach (Move m in Form.Pool[l])
				{
					if (slot >= MoveSet.Length) break;
					MoveSet[slot++].Move = m;
				}
			
			Heal();
		}

		private Item HoldItem()
		{
			int n = R.Next(100);
			Item common = null, uncommon = null, rare = null;
			if (Form.HoldItems.ContainsKey("common")) common = Form.HoldItems["common"];
			if (Form.HoldItems.ContainsKey("uncommon")) uncommon = Form.HoldItems["uncommon"];
			if (Form.HoldItems.ContainsKey("rare")) rare = Form.HoldItems["rare"];

			if (common == uncommon && uncommon == rare)
				return common;

			if (n < 50) return common;
			else if (n < 55) return uncommon;
			else if (n < 56) return rare;
			else return null;
		}

		private int S(Stat i)
		{
			var baseS = Form.BaseStats;
			if (i == "hp")
			{
				if (baseS[i] == 1) return 1;
				return 10 + (baseS[i] * 2 + IV[i] + EV[i] / 4) * Level / 100 + Level;
			}
			return 5 + (int)((baseS[i] * 2 + IV[i] + EV[i] / 4) * Level / 100.0 * Nature[i]);
		}

		[JsonProperty]
		private readonly int id;
		[JsonProperty]
		private string _species;
		[JsonProperty]
		private string _form;
		[JsonProperty]
		private string _name;
		[JsonProperty]
		private double _gender;
		[JsonProperty]
		private int _shiny;
		[JsonProperty]
		public byte Happiness;
		[JsonProperty]
		private int _experience;
		[JsonProperty]
		private string _item;
		[JsonProperty]
		private byte[] _iv, _ev;
		[JsonProperty]
		private string _nature;
		[JsonProperty]
		private string _ability;
		[JsonProperty]
		public readonly DateTime TimeReceived;
		[JsonProperty]
		public readonly int MeetLevel;
		[JsonProperty]
		private MoveSlot[] _MoveSet;
		[JsonProperty]
		private int _HP;
		public Trainer Trainer;
	}
}
