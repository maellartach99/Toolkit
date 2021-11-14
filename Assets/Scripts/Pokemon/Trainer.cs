using System;
using Pokemon.Data;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Pokemon
{
	[JsonObject]
	public class Trainer
	{
		public static readonly Random R = Pokemon.R;
		public static readonly int PartyLength = 6;

		[JsonProperty]
		public string Name { get; set; }
		[JsonIgnore]
		public string FullName { get => Name + " " + Class.Name; }
		[JsonIgnore]
		public TrainerClass Class { get { return (TrainerClass)tclass; } }
		[JsonIgnore]
		public int PublicID { get { return id & 0xFFFF; } }
		[JsonIgnore]
		public int SecretID { get { return id >> 16; } }
		[JsonProperty]
		public Bag Bag { get; private set; }
		public Pokemon this[int index]
		{
			get { return party[index]; }
			set { if (index < Count) party[index] = value; }
		}
		
		[JsonIgnore]
		public int Count { get { return party.Count; } }
		public virtual void Add(Pokemon pkm)
		{
			if (Count < PartyLength)
				party.Add(pkm);
		}

		public Trainer(string name, string tclass)
		{
			id = R.Next(256) | R.Next(256) << 8 | R.Next(256) << 16 | R.Next(256) << 24;
			Name = name;
			party = new List<Pokemon>();
			this.tclass = tclass;
			Bag = new Bag();
		}

		[JsonProperty]
		protected int id;
		[JsonProperty]
		protected string tclass;
		[JsonProperty]
		protected List<Pokemon> party;
	}


	public class Player : Trainer
	{
		public static readonly int InitialMoney = 3000;
		public static readonly int BoxNum = 48;

		[JsonProperty]
		public int Money { get; set; }
		[JsonProperty]
		public int Coins { get; set; }
		[JsonProperty]
		public bool[] Badges { get; private set; }

		public Player(string name, string tclass) : base(name, tclass)
		{
			Money = InitialMoney;
			Coins = 0;
			Badges = new bool[8];

			pc = new Box[BoxNum];
			for (int i = 0; i < BoxNum; i++)
				pc[i] = new Box("Caja "+(i+1));
		}
		public Pokemon this[int box, int index]
		{
			get { return pc[box][index]; }
			set { pc[box][index] = value; }
		}
		public override void Add(Pokemon pkm)
		{
			if (Count < PartyLength)
				party.Add(pkm);
			else
			{
				for (int i = 0; i < BoxNum; i++)
					if (pc[i].Add(pkm)) break;
			}
		}
		public Box Box(int i) => pc[i];

		[JsonProperty]
		private Box[] pc;
	}
}
