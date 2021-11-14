using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.UI
{
	public class SaveScreen : SceneUI
	{
		public Image[] party;

		// Start is called before the first frame update
		void Start()
		{
			var player = Game.Data.Player;
			for (int i = 0; i < party.Length; i++)
			{
				if (i < player.Count)
				{
					party[i].gameObject.SetActive(true);
					party[i].sprite = Resources.Load<Sprite>(player[i].Sprite);
				}
				else party[i].gameObject.SetActive(false);
			}
		}

		// Update is called once per frame
		public override void update()
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				Game.SaveGame();
				UI.Remove();
			}
			if (Input.GetKeyDown(KeyCode.S))
				UI.Remove();
		}
	}
}
