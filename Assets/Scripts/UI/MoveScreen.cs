using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.UI
{
	public class MoveScreen : SceneUI
	{
		public Text[] names, pp;
		public Image[] types, move;
		public Image category, catIcon;
		public Text power, acc, desc;
		public RectTransform selection;

		private int _index;
		public int Index
		{
			get => _index;
			set { _index = (value + 4) % 4; }
		}
		Pokemon pkm { get => Game.Data.Player[(UI.Last as SummaryScreen).Index]; }

		// Start is called before the first frame update
		void Start()
		{
			
		}

		// Update is called once per frame
		public override void update()
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
				Index--;
			if (Input.GetKeyDown(KeyCode.DownArrow))
				Index++;

			if (Input.GetKeyDown(KeyCode.Z))
				UI.Remove();
			if (Input.GetKeyDown(KeyCode.S))
				UI.Remove();
			
			Refresh();
		}

		private void Refresh()
		{
			for (int i = 0; i < 4; i++)
			{
				Data.MoveSlot mv = pkm.MoveSet[i];
				move[i].gameObject.SetActive(mv.Move != null);
				if (mv.Move == null) continue;

				move[i].color = mv.Move.Type.Color;
				names[i].text = mv.Move.Name;
				pp[i].text = mv.PP + "/" + mv.MaxPP;
				types[i].sprite = Resources.Load<Sprite>(mv.Move.Type.Icon);
			}

			Data.Move mov = pkm.MoveSet[Index].Move;
			if (mov != null)
			{
				switch (mov.Category)
				{
					case Data.Category.Physical:
						category.color = new Color(192/255.0f,48/255.0f,40/255.0f);
						catIcon.sprite = Resources.Load<Sprite>("Icons/physical");
						break;
					case Data.Category.Special:
						category.color = new Color(104/255.0f,144/255.0f,240/255.0f);
						catIcon.sprite = Resources.Load<Sprite>("Icons/special");
						break;
					case Data.Category.Status:
						category.color = Color.white;
						catIcon.sprite = Resources.Load<Sprite>("Icons/status");
						break;
				}
				power.text = mov.BasePower == 0 ? "---" : mov.BasePower + "";
				acc.text = mov.Accuracy == 255 ? "---" : mov.Accuracy + "";
				desc.text = mov.Description;
			}

			selection.anchorMin = new Vector2(selection.anchorMin.x, 0.57f - 0.1f * Index);
			selection.anchorMax = new Vector2(selection.anchorMax.x, 0.65f - 0.1f * Index);
		}
	}
}