using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : Command
{
	public GameObject obj;

	public override IEnumerator Execute()
	{
		if (obj == null)
			obj = gameObject;

		obj.SetActive(false);
		yield return null;
	}
}
