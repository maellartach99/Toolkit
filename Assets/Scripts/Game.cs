using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.IO;

/// <summary>
/// Clase que representa una partida del juego.
/// Contiene los datos del entrenador y variables globales de distintos tipos.
/// Es posible crear una partida nueva, guardar una partida en un fichero,
/// o leer la partida de un fichero.
/// </summary>
[JsonObject]
public class Game
{
	[JsonProperty]
	private Dictionary<string,bool> booleans;
	[JsonProperty]
	private Dictionary<string,int> integers;
	[JsonProperty]
	private Dictionary<string,double> reals;
	[JsonProperty]
	private Dictionary<string,string> strings;
	[JsonProperty]
	public Pokemon.Player Player { get; set; }

	[JsonProperty]
	public Vector3 Position { get; private set; }
	[JsonProperty]
	public string Scene { get; private set; }

	[JsonConstructor]
	private Game(string name, string tclass)
	{
		booleans = new Dictionary<string, bool>();
		integers = new Dictionary<string, int>();
		reals = new Dictionary<string, double>();
		strings = new Dictionary<string, string>();
		Player = new Pokemon.Player(name,tclass);
	}

	public static void NewGame(string name, string tclass)
	{
		Data = new Game(name,tclass);

		/*
		var p = Game.Data.Player;
		p.Add(new Pokemon.Pokemon("pikachu", 25));
		p.Add(new Pokemon.Pokemon("torterra", 48));
		p.Add(new Pokemon.Pokemon("buizel", 10));
		p.Add(new Pokemon.Pokemon("staraptor", 48));
		p.Add(new Pokemon.Pokemon("gliscor", 50));
		p.Add(new Pokemon.Pokemon("infernape", 52));
		foreach (string especie in Pokemon.Data.Species.Names)
		{
			var pkm = new Pokemon.Pokemon(especie, 50);
			p.Add(pkm);
			foreach (string forma in pkm.Species.Forms)
			{
				pkm = new Pokemon.Pokemon(especie,50,forma);
				p.Add(pkm);

			}
		}
		foreach (string s in Pokemon.Data.Item.Names)
			p.Bag.Add((Pokemon.Data.Item)s,1);
		*/
	}

	public static void SaveGame()
	{
		Data.Scene = SceneManager.GetActiveScene().name;
		Data.Position = PlayerScript.Instance.transform.position;

		string save = JsonConvert.SerializeObject(Data);
		File.WriteAllText(Application.persistentDataPath + "/save.json", save);
		Debug.Log(Application.persistentDataPath);
	}

	public static void LoadGame()
	{
		string json = File.ReadAllText(Application.persistentDataPath + "/save.json");
		Data = JsonConvert.DeserializeObject<Game>(json);
	}

	public static Game Data { get; private set; }
}
