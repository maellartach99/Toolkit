using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Teleport : Command
{
	public bool relative;
	public string scene;
	public Vector3 position;
	
	public override IEnumerator Execute()
	{
		yield return To(relative,scene,position);
	}

	public static IEnumerator To(bool relative,string scene,Vector3 position)
	{
		if (scene != "")
		{
			var asyncLoad = SceneManager.LoadSceneAsync(scene);
			yield return new WaitUntil( () => asyncLoad.isDone);
		}

		if (relative)
			Player.GetComponent<Event>().Move(Player.transform.position + position);
		else Player.GetComponent<Event>().Move(position);
	}
}
