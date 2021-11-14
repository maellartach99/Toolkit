using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	public Text nameText;
	public Text dialogueText;
	public Image nameBox;
	public Image dialogueBox;

	private Queue<string> sentences;
	private bool typed = false;
	public bool Ending { get => (sentences.Count == 0) && typed; }
	
	public static DialogueManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(this.gameObject);
		else
		{
			Instance = this;
			DontDestroyOnLoad(transform.parent);
		}
	}

	// Use this for initialization
	void Start()
	{
		sentences = new Queue<string>();
	}

	public void StartDialogue(string[] sent, string name)
	{
		nameBox.gameObject.SetActive(name != "");
		dialogueBox.gameObject.SetActive(true);

		nameText.text = name;
		sentences.Clear();

		foreach (string sentence in sent)
			sentences.Enqueue(sentence);

		DisplayNext();
	}

	public void DisplayNext()
	{
		if (sentences.Count == 0)
		{
			Finish();
			return;
		}

		string sentence = sentences.Dequeue();
		typed = false;
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
		typed = true;
	}

	void Finish()
	{
		nameBox.gameObject.SetActive(false);
		dialogueBox.gameObject.SetActive(false);
	}

}
