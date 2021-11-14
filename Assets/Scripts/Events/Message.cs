using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : Command
{
	public string Name;

	[TextArea(3, 10)]
	public string[] Sentences;

	public override IEnumerator Execute()
	{
		yield return Show(Name,Sentences);
	}

	public static IEnumerator Show(string Name, string[] Sentences)
	{
		DM.StartDialogue(Sentences,Name);
		yield return null;

		while (!DM.Ending)
		{
			if (Input.GetKeyDown(KeyCode.A))
				DM.DisplayNext();
			yield return null;
		}

		while (!Input.GetKeyUp(KeyCode.A))
			yield return null;
		
		DM.DisplayNext();
	}

	private static DialogueManager DM { get => DialogueManager.Instance; }
}
