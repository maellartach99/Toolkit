using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que controla al Pokémon que sigue al jugador.
/// Utiliza el patrón Singleton, sólo existe una instancia de esta clase.
/// </summary>
public class PokemonFollow : Command
{
	public static PokemonFollow Instance;

	public Transform Following;
	private Pokemon.Pokemon Pokemon;
	private GameObject Model;

	private void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(this.gameObject);
		else
		{
			Instance = this;
			DontDestroyOnLoad(Instance);
		}
	}

    public override IEnumerator Execute()
	{
		transform.LookAt(Following);

		if (Vector3.Distance(transform.position,Following.position) > 5)
			Event.Move(Following.position - transform.forward);
		
		Event.speed = Following.GetComponent<Event>().speed;

		Event.target = Following.position - Following.Find("Model").forward;
		
		if (Game.Data.Player.Count == 0)
			yield return null;
		
		if (Pokemon != Game.Data.Player[0])
		{
			Pokemon = Game.Data.Player[0];
			Destroy(Model);
			var original = Resources.Load<GameObject>(Pokemon.Model);
			Model = Instantiate<GameObject>(original);
			Model.transform.parent = transform;
			Model.transform.localPosition = -Model.transform.forward * Model.Max().z / 100;
			Model.transform.localEulerAngles = Vector3.zero;
			if (Pokemon.Form.Height < 1)
				Model.transform.localScale /= (float)Pokemon.Form.Height / 0.75f;
		}
	}
}
