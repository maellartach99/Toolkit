using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pokemon.UI;

[System.Serializable]
public abstract class Command : MonoBehaviour
{
	public abstract IEnumerator Execute();
	public bool Finished { get; set; }
	public IEnumerator StartCommand()
	{
		yield return Execute();
		Finished = true;
	}

	public static GameObject Player { get => GameObject.FindWithTag("Player"); }
	public static UserInterface UI { get => UserInterface.Instance; }
	public Event Event { get => gameObject.GetComponent<Event>(); }
}
