using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.UI
{
	public class PartyScreen : SceneUI
	{
		public Image[] buttons, sprite, hpbar;
		public Text[] names, level, hp;

		public int index;
		private int swap = -1;

		public void Swap()
		{
			if (swap == -1)
				swap = index;
			else
			{
				Pokemon temp = Game.Data.Player[swap];
				Game.Data.Player[swap] = Game.Data.Player[index];
				Game.Data.Player[index] = temp;
				swap = -1;
			}
		}

		// Start is called before the first frame update
		void Start()
		{
			var player = Game.Data.Player;
			PokemonView.Instance.Change(player[index]);
		}

		// Update is called once per frame
		public override void update()
		{
			var player = Game.Data.Player;

			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				index = (index - 1 + player.Count) % player.Count;
				PokemonView.Instance.Change(player[index]);
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				index = (index + 1) % player.Count;
				PokemonView.Instance.Change(player[index]);
			}

			for (int i = 0; i < buttons.Length; i++)
			{
				if (i < player.Count)
				{
					buttons[i].gameObject.SetActive(true);
					Color color = new Color(1,1,1,1);
					if (i != index) color.a = 0.5f;
					if (i == swap) {color.r = 0; color.g = 0.5f;}
					buttons[i].color = color;
					sprite[i].sprite = Resources.Load<Sprite>(player[i].Sprite);
					names[i].text = player[i].Name;
					hpbar[i].color = bar(player[i]);
					var rect = hpbar[i].GetComponent<RectTransform>();
					float hp = 0.4f + player[i].HP/(float)player[i].Stats["hp"] * (0.9f-0.4f);
					rect.anchorMax = new Vector2(hp,rect.anchorMax.y);

					level[i].text = "Nv " + player[i].Level;
					this.hp[i].text = player[i].HP + "/" + player[i].Stats["hp"];
				}
				else buttons[i].gameObject.SetActive(false);
			}

			if (Input.GetKeyDown(KeyCode.A))
			{
				if (swap != -1) Swap();
				else UI.Add("Party Screen Menu");
			}
			if (Input.GetKeyDown(KeyCode.Z))
				UI.Remove();
			if (Input.GetKeyDown(KeyCode.S))
				UI.Remove();
		}

		public static Color bar(Pokemon pkm)
		{
			if (pkm.HP > pkm.Stats["hp"]/2)
				return Color.green;
			else if (pkm.HP > pkm.Stats["hp"]/4)
				return Color.yellow;
			else return Color.red;
		}
	}
}