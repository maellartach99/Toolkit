using System;
using Newtonsoft.Json;

namespace Pokemon.Data
{
	[JsonObject]
	public class MoveSlot
	{
		Func<int, int, int, int> Lim = Pokemon.Lim;

		[JsonIgnore]
		public Move Move 
		{
			get => (_move == null) ? null : (Move)_move;
			set
			{
				_move = value.ID;
				PPUps = 0;
				if (value != null)
					_pp = (PP > BasePP) ? BasePP : PP;
				else _pp = 255;
			}
		}

		[JsonIgnore]
		public byte BasePP { get => Move.PP;  }
		[JsonIgnore]
		public byte MaxPP { get { return (byte)(BasePP + (BasePP / 5) * PPUps); } }
		[JsonIgnore]
		public byte PPUps 
		{
			get => _ppups;
			set { _ppups = (byte)Lim(0,value,3); } 
		}
		[JsonIgnore]
		public byte PP
		{
			get => _pp;
			set { _pp = (byte)Lim(0,value, MaxPP); }
		}

		[JsonProperty]
		private byte _ppups = 0, _pp = 255;
		[JsonProperty]
		private string _move = null;
	}
}
