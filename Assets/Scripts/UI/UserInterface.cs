using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.UI
{
	public class UserInterface : MonoBehaviour
	{
		private Stack<SceneUI> SceneStack;
		public SceneUI Last { get; private set; }
		public static UserInterface Instance { get; private set; }

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

		void Start()
		{
			SceneStack = new Stack<SceneUI>();
		}

		public void Add(string name)
		{
			GameObject obj = Instantiate(Resources.Load<GameObject>("UI/"+name));
			var rect = obj.GetComponent<RectTransform>();
			obj.transform.SetParent(transform);
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;

			Last = Scene;
			SceneStack.Push(obj.GetComponent<SceneUI>());
			Event.Wait = true;
		}

		public void Remove()
		{
			if (Scene != null)
			{
				Last = SceneStack.Pop();
				Destroy(Last.gameObject);
				if (Scene == null) Event.Wait = false;
			}
		}

		public SceneUI Scene
		{
			get
			{
				try { return SceneStack.Peek(); }
				catch { return null; }
			}
		}
	}
}
