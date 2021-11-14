using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pokemon.Data
{
    public class Move
    {
        private static Dictionary<string, Move> dictionary;
		public static string[] Names { get => new List<string>(dictionary.Keys).ToArray(); }

        public string ID { get; private set; }
		[JsonIgnore] public int Num { get => num; }
		[JsonIgnore] public string Name { get => name; }
		[JsonIgnore] public string Description { get => description; }
		[JsonIgnore] public byte Accuracy { get => accuracy; }
		[JsonIgnore] public byte BasePower { get => basePower; }
		[JsonIgnore] public byte PP { get => pp; }
		[JsonIgnore] public int Priority { get => priority; }
		[JsonIgnore] public byte CriticalRatio { get => critRatio; }
		[JsonIgnore] public Category Category { get => (Category) Enum.Parse(typeof(Category),category,true); }
		[JsonIgnore] public Type Type { get => (Type)type; }
		[JsonIgnore] public Flags Flags
		{
			get
			{
				Flags result = 0;
				foreach (var pair in flags)
					result |= (Flags)Enum.Parse(typeof(Flags), pair.Key, true);
				return result;
			}
		}
		[JsonIgnore] public byte[] MultiHit { get => (byte[])multihit.Clone(); }
		[JsonIgnore] public byte[] Drain { get => (byte[])drain.Clone(); }
		[JsonIgnore] public byte[] Recoil { get => (byte[])recoil.Clone(); }

		public static explicit operator string(Move m) { return m.ID; }
		public static explicit operator Move(string s) { return dictionary[s]; }

		public static Type HiddenPower(Stats iv)
		{
			string[] types = Type.Names;
			double t = 0;

			foreach (var pair in iv) // 0 <= t < 64
			{
				int i = pair.Key;    // 0 <= i < 6
				int val = pair.Value;
				t += Math.Pow(2,i) * (val%2);
			}

			t *= (types.Length - 2) / 63.0; // 0 <= t <= |Types|-1
			return (Type)types[(int)t+1];       // 1 <= t <= |Types|
		}

		[JsonConstructor] private Move() { }

		static Move()
		{
			string json = Read.Data("move");
			dictionary = JsonConvert.DeserializeObject< Dictionary<string,Move> >(json);

			foreach (var pair in dictionary)
				pair.Value.ID = pair.Key;
		}

		[JsonProperty] private int num { get; set; }
		[JsonProperty] private string name { get; set; }
		[JsonProperty] private string description { get; set; }
		[JsonProperty] private byte accuracy { get; set; }
		[JsonProperty] private byte basePower { get; set; }
		[JsonProperty] private byte pp { get; set; }
		[JsonProperty] private int priority { get; set; }
		[JsonProperty] private byte critRatio { get; set; }
		[JsonProperty] private string category { get; set; }
		[JsonProperty] private string type { get; set; }
		[JsonProperty] private string contestType { get; set; }
		[JsonProperty] private Dictionary<string,byte> flags { get; set; }
		[JsonProperty] private object secondary { get; set; }
		[JsonProperty] private byte[] multihit { get; set; }
		[JsonProperty] private byte[] drain { get; set; }
		[JsonProperty] private byte[] recoil { get; set; }
    }

    public enum Category
    {
        Physical,
        Special,
        Status
    }

    [Flags]
    public enum Flags
    {
        bite        = 0x00001,   // Potencia multiplicada por 1.5 combinado con la habilidad Mandíbula Fuerte.
        bullet      = 0x00002,   // No tiene efecto en Pokemon con la habilidad Bulletproof.
        charge      = 0x00004,   // El movimiento tiene que cargar el primer turno.
        contact     = 0x00008,   // Hace contacto.
        dance       = 0x00010,   // Afectado por la habilidad Pareja de Baile.
        defrost     = 0x00020,   // Libera de la congelación al usarse.
        distance    = 0x00040,   // Can target a Pokemon positioned anywhere in a Triple Battle.
        gravity     = 0x00080,   // No puede ser ejecutado bajo los efectos de Gravedad.
        heal        = 0x00100,   // No puede ser ejecutado bajo los efectos de Anticura.
        mirror      = 0x00200,   // Puede ser copiado por Movimiento Espejo.
        mystery     = 0x00400,   // Efecto desconocido.
        nonsky      = 0x00800,   // Prevented from being executed or selected in a Sky Battle.
        powder      = 0x01000,   // No tiene efecto en Pokemon tipo Planta, Pokemon con la habilidad Overcoat, y Pokemon llevando Safety Goggles.
        protect     = 0x02000,   // Bloqueado por Detección, Protección, Barrera Espinosa, y si no es un movimiento de Estado, Escudo Real.
        pulse       = 0x04000,   // Power is multiplied by 1.5 when used by a Pokemon with the Mega Launcher Ability.
        punch       = 0x08000,   // Power is multiplied by 1.2 when used by a Pokemon with the Iron Fist Ability.
        recharge    = 0x10000,   // Necesita un turno para recargar despues de usarse.
        reflectable = 0x20000,   // Rebota por el movimiento Magic Coat o la habilidad Magic Bounce.
        snatch      = 0x40000,   // Los efectos del movimiento pueden ser robados con el movimiento Robo.
        sound       = 0x80000,   // No tiene efecto en Pokemon con la habilidad Insonorizar.
		authentic   = 0x100000   // Ignora el sustituto.
	}

    public enum Target
    {
        adjacentAlly,
        adjacentAllyOrSelf,
        adjacentFoe,
        all,
        allAdjacent,
        allAdjacentFoes,
        allies,
        allySide,
        allyTeam,
        any,
        foeSide,
        normal,
        randomNormal,
        scripted,
        self
    }
}
