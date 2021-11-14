using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.UI
{
	public class LoadScreen : SceneUI
	{
		public Image[] Buttons;
		public int Index
		{
			get => _index;
			set => _index = (value + 4) % 4;
		}
		private int _index;

		// Update is called once per frame
		public override void update()
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
				Index--;
			if (Input.GetKeyDown(KeyCode.DownArrow))
				Index++;

			if (Input.GetKeyDown(KeyCode.A))
			{
				switch (Index)
				{
					case 0:
						loadgame();
						break;
					case 1:
						newgame();
						break;
					case 2: break;
					case 3:
						Application.Quit();
						break;
				}
			}

			for (int i = 0; i < 4; i++)
			{
				if (i == Index)
					Buttons[i].color = new Color(1,1,1,1);
				else Buttons[i].color = new Color(1,1,1,0.5f);
			}
		}

		void newgame()
		{
			Game.NewGame("Serena","leaf");
			UI.Remove();
			UI.StartCoroutine(Teleport.To(false,"Sureste",Vector3.zero));
		}

		void loadgame()
		{
			Game.LoadGame();
			UI.Remove();
			UI.StartCoroutine(Teleport.To(false,Game.Data.Scene,Game.Data.Position));
		}
	}
}
