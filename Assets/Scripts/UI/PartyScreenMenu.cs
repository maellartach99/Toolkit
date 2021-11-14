using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.UI
{
	public class PartyScreenMenu : SceneUI
	{
		public Image[] buttons;

		private int index;
		private int pokemon;

		// Start is called before the first frame update
		void Start()
		{
			pokemon = (UI.Last as PartyScreen).index;
		}

		// Update is called once per frame
		public override void update()
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
				index = (index - 1 + buttons.Length) % buttons.Length;
			if (Input.GetKeyDown(KeyCode.DownArrow))
				index = (index + 1) % buttons.Length;
			
			for (int i = 0; i < buttons.Length; i++)
			{
				if (i == index)
					buttons[i].color = new Color(1,1,1,1);
				else buttons[i].color = new Color(1,1,1,0.5f);
			}

			if (Input.GetKeyDown(KeyCode.A))
				Action();
			if (Input.GetKeyDown(KeyCode.Z))
				UI.Remove();
			if (Input.GetKeyDown(KeyCode.S))
				UI.Remove();
		}

		private void Action()
		{
			UI.Remove();

			switch (index)
			{
				case 0:
					UI.Add("Summary Screen");
					var summary = UI.Scene.GetComponent<SummaryScreen>();
					summary.Index = pokemon;
					break;
				case 1: (UI.Scene as PartyScreen).Swap(); break;
				case 2: break;
				case 3: break;
			}
		}
	}
}

