using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pokemon.UI;

public class Interface : Command
{
	public string screen;

	public override IEnumerator Execute()
	{
		UserInterface.Instance.Add(screen);
		yield return new WaitUntil( () => UserInterface.Instance.Scene == null );
	}
}
