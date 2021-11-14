using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public static Vector3 Min(this GameObject obj)
	{
		MeshFilter[] meshes = obj.GetComponentsInChildren<MeshFilter>();
		int len = meshes.Length;
         
		var x = new List<float>();
		var y = new List<float>();
		var z = new List<float>();
         
		for (int i = 0; i < len; i++)
		{
			// X
			float actualX = meshes[i].mesh.bounds.min.x;
			x.Add(actualX);
             
			// Y
			float actualY = meshes[i].mesh.bounds.min.y;
			y.Add(actualY);
             
			// Z
			float actualZ = meshes[i].mesh.bounds.min.z;
			z.Add(actualZ);
		}

		x.Sort();
		y.Sort();
		z.Sort();

		return new Vector3(x[0],y[0],z[0]);
	}

	public static Vector3 Max(this GameObject obj)
	{
		MeshFilter[] meshes = obj.GetComponentsInChildren<MeshFilter>();
		int len = meshes.Length;
         
		var x = new List<float>();
		var y = new List<float>();
		var z = new List<float>();
         
		for (int i = 0; i < len; i++)
		{
			// X
			float actualX = meshes[i].mesh.bounds.max.x;
			x.Add(actualX);
             
			// Y
			float actualY = meshes[i].mesh.bounds.max.y;
			y.Add(actualY);
             
			// Z
			float actualZ = meshes[i].mesh.bounds.max.z;
			z.Add(actualZ);
		}

		x.Sort();
		y.Sort();
		z.Sort();

		return new Vector3(x[len-1],y[len-1],z[len-1]);
	}
}

public class PokemonView : MonoBehaviour
{
	public static PokemonView Instance { get; private set; }

	public void Change(Pokemon.Pokemon pkm)
	{
		if (Pokemon != null) Destroy(Pokemon);
		if (pkm == null) return;
		var original = Resources.Load<GameObject>(pkm.Model);
		if (original == null) return;
		Pokemon = Instantiate<GameObject>(original);
		Pokemon.transform.parent = transform;
		Pokemon.transform.localPosition = Vector3.zero;
		Pokemon.transform.localEulerAngles = Vector3.zero;
		if (pkm.Form.Height < 1)
			Pokemon.transform.localScale /= (float)pkm.Form.Height;
	}

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

	public GameObject Pokemon;
}
