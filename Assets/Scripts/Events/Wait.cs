using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : Command
{
	public float Time;

	public override IEnumerator Execute()
	{
		yield return new WaitForSecondsRealtime(Time);
	}
}
