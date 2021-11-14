using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.UI
{
	public class BoxScreen : SceneUI
	{
		private int boxNum;
		private int x, y;
		private Player Player { get => Game.Data.Player; }
		private int index { get => (x < 6) ? (6*y + x) : (2*y+x-10); }

		public Text BoxName;
		public RectTransform Selection;
		public Image[] Box;
		public Image[] Party;

		// Update is called once per frame
		public override void update()
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if (y == -1) boxNum = (boxNum - 1 + Player.BoxNum) % Player.BoxNum;
				else if (y < 2) x = (x + 5) % 6;
				else x = (x + 7) % 8;

				Refresh();
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				if (y == -1) boxNum = (boxNum + 1) % Player.BoxNum;
				else if (y < 2) x = (x + 1) % 6;
				else x = (x + 1) % 8;

				Refresh();
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				y--;
				if (x < 6)
				{
					if (y < -1) y += 6;
				}
				else if (y < 2) y += 3;

				Refresh();
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				y++;
				if (y > 4)
					y = (x < 6) ? -1 : 2;
				
				Refresh();
			}

			if (Input.GetKeyDown(KeyCode.Z))
				UI.Remove();
			if (Input.GetKeyDown(KeyCode.S))
				UI.Remove();

		}

		void Refresh()
		{
			BoxName.text = Player.Box(boxNum).Name;

			for (int i = 0; i < Box.Length; i++)
			{
				Pokemon pkm = Player[boxNum,i];
				Box[i].gameObject.SetActive(pkm != null);

				if (pkm != null)
					Box[i].sprite = Resources.Load<Sprite>(pkm.Sprite);
			}
			for (int i = 0; i < Party.Length; i++)
			{
				Pokemon pkm = Player[i];
				Party[i].gameObject.SetActive(pkm != null);

				if (pkm != null)
					Party[i].sprite = Resources.Load<Sprite>(pkm.Sprite);
			}

			if (x < 6)
			{
				Selection.anchorMin = new Vector2(0,0);
				Selection.anchorMax = new Vector2(0,0);
				Selection.pivot = new Vector2(-x,y-4);
			}
			else
			{
				Selection.anchorMin = new Vector2(1,0);
				Selection.anchorMax = new Vector2(1,0);
				Selection.pivot = new Vector2(-x+8,y-4);
			}

			Selection.gameObject.SetActive(y != -1);
			var img = BoxName.transform.parent.GetComponent<Image>();
			img.color = (y != -1) ? new Color(1,1,1,0.5f) : new Color(1,1,1,1);

			if (y < 0) return;
			if (x < 6) PokemonView.Instance.Change(Player[boxNum,index]);
			else PokemonView.Instance.Change(Player[index]);
		}
	}
}

