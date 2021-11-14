using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.UI
{
	public abstract class SceneUI : MonoBehaviour
	{
		public UserInterface UI { get => FindObjectOfType<UserInterface>(); }

		void Update()
		{
			if (UI.Scene == this)
				update();
		}

		public abstract void update();
		public virtual object result() => null;
	}
}
