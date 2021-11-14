using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenColor : Command
{
	public Color Color;
	public float Seconds;

	public override IEnumerator Execute()
	{
		yield return Change(Color,Seconds);
	}

	public static IEnumerator Change(Color Color, float Seconds)
	{
		float time = 0;
		if (screen == null) screen = GetScreen();
		Color original = screen.color;
		while (time < Seconds)
		{
			time += Seconds/16;
			screen.color = Color.Lerp(original,Color,time/Seconds);
			yield return new WaitForSecondsRealtime(Seconds/16);
		}

		screen.color = Color;
	}

	private static Image screen; 

	private static Image GetScreen()
	{
		foreach (var canvas in FindObjectsOfType<Canvas>())
		{
			if (canvas.name == "Canvas")
			{
				DontDestroyOnLoad(canvas);
				var transf = canvas.transform.Find("Screen Color");
				return transf.GetComponent<Image>();
			}
		}
		return null;
	}
}
