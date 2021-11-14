using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Pokemon.Data
{
	public class Species
	{
		private static Dictionary<string, Species> dictionary;
		public static string[] Names { get => new List<string>(dictionary.Keys).ToArray(); }

		public static explicit operator string(Species s) { return s.ID; }
		public static explicit operator Species(string s) { return dictionary[s]; }

		[JsonIgnore] public string ID { get; private set; }
		[JsonIgnore] public int Num { get => num; }
		[JsonIgnore] public string Name { get => name; }
		[JsonIgnore] public double GenderRate { get => genderRate; }
		[JsonIgnore] public bool GenderDiff { get => genderDiff; }
		[JsonIgnore] public Experience Experience { get => new Experience(experience); }
		[JsonIgnore] public int BaseExp { get => baseExp; }
		[JsonIgnore] public byte Rareness { get => rareness; }
		[JsonIgnore] public byte Happiness { get => happiness; }
		[JsonIgnore] public Egg[] Group { get; }
		[JsonIgnore] public int EggSteps { get => eggSteps; }

		[JsonIgnore] public Form Base { get; private set; }
		[JsonIgnore] public string[] Forms { get => Form.Alternative(ID); }
		public Form this[string name]
		{
			get
			{
				if (Forms.Contains(name))
					return Form.Alternative(ID,name);
				return Base;
			}
		}

		[JsonConstructor] private Species() { }

		static Species()
		{
			string json = Read.Data("species");
			dictionary = JsonConvert.DeserializeObject< Dictionary<string,Species> >(json);
			var formas = JsonConvert.DeserializeObject< Dictionary<string,Form> >(json);

			foreach (var pair in dictionary)
			{
				pair.Value.ID = pair.Key;
				pair.Value.Base = formas[pair.Key];
				pair.Value.Base.species = pair.Key;
				if (!pair.Value.test())
					throw new Exception("Species "+pair.Key);
			}
		}

		private bool test()
		{
			if (num == default) return false;
			if (name == null) return false;
			if (experience == null) return false;
			if (baseExp == default) return false;
			if (rareness == default) return false;
			if (eggSteps == default) return false;
			if (!Base.test()) return false;
			return true;
		}

		[JsonProperty] private int num;
		[JsonProperty] private string name;
		[JsonProperty] private double genderRate;
		[JsonProperty] private bool genderDiff;
		[JsonProperty] private string experience;
		[JsonProperty] private int baseExp;
		[JsonProperty] private byte rareness;
		[JsonProperty] private byte happiness;
		[JsonProperty] private string[] group;
		[JsonProperty] private int eggSteps;
	}

	public enum Egg
	{
		Monster,
		Water1,      // Anfibios
		Bug,         // Insectoides
		Flying,      // Aves y pájaros
		Field,       // Terrestres
		Fairy,       // Pequeños y cute
		Grass,       // De naturaleza vegetal
		Human,       // Bipedos y/o antropomorfos
		Water3,      // Invertebrados acuáticos
		Mineral,     // De naturaleza inorgánica
		Amorphous,   // Sin forma definida
		Water2,      // Peces
		Ditto,       // Puede criar con cualquiera
		Dragon,      // Apariencia draconiana
		Undiscovered // No puede criar, ni siquiera con Ditto
	}
}
