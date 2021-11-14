using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.UI
{

	public class PauseMenu : SceneUI
	{
		int x, y;
		int index { get => 2*y + x; }
		static string[] screens = {"Box Screen","Party Screen","Bag Screen",null,"Save Screen",null};

		public Image[] Buttons;

		// Start is called before the first frame update
		void Start()
		{
			var name = transform.Find("Player").Find("Text").GetComponent<Text>();
			name.text = Game.Data.Player.Name;
		}

		public override void update()
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
				y = (y + 2) % 3;
			if (Input.GetKeyDown(KeyCode.DownArrow))
				y = (y + 1) % 3;
			if (Input.GetKeyDown(KeyCode.LeftArrow))
				x = (x + 1) % 2;
			if (Input.GetKeyDown(KeyCode.RightArrow))
				x = (x + 1) % 2;

			if (Input.GetKeyDown(KeyCode.Z))
				UI.Remove();
			if (Input.GetKeyDown(KeyCode.S))
				UI.Remove();

			if (Input.GetKeyDown(KeyCode.A))
			{
				if (index == 4) UI.Remove();
				UI.Add(screens[index]);
			}
			
			Refresh();
		}

		public void Refresh()
		{

			for (int i = 0; i < Buttons.Length; i++)
			{
				if (i == index)
					Buttons[i].color = Color.white;
				else Buttons[i].color = new Color(1,1,1,0.5f);
			}
		}
	}
}
