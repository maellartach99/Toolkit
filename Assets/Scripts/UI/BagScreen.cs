using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pokemon.Data;

namespace Pokemon.UI
{
	public class BagScreen : SceneUI
	{
		private Player Player { get => Game.Data.Player; }
		private Bag Bag { get => Game.Data.Player.Bag; }
		private Pocket Pocket { get => Bag[Bag.Pockets[pocketNum]]; }
		private int Index
		{
			get => indexes[pocketNum];
			set => indexes[pocketNum] = value;
		}
		private int Offset
		{
			get => offsets[pocketNum];
			set => offsets[pocketNum] = value;
		}
		private int[] indexes, offsets;
		private int pocketNum = 0;

		public Text pocket, description;
		public Image[] pocketIcons;
		public Image[] labels, icons;
		public Text[] names, nums;
		public Image[] partyLabel, partySprite, hpBars;
		public Text[] partyNames, levels, hp;

		// Start is called before the first frame update
		void Start()
		{
			indexes = new int[Bag.Pockets.Length];
			offsets = new int[Bag.Pockets.Length];
			Refresh();
		}

		// Update is called once per frame
		public override void update()
		{
			bool change = false;
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				pocketNum = (pocketNum <= 0 ? pocketIcons.Length : pocketNum) - 1;
				change = true;
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				pocketNum = (pocketNum + 1) % pocketIcons.Length;
				change = true;
			}
			
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				Index = (Index - 1 + Pocket.Count) % Pocket.Count;
				if (Index < Offset) Offset = Index;
				if (Index - Offset >= names.Length)
					Offset = Index - names.Length + 1;
				change = true;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				Index = (Index + 1) % Pocket.Count;
				if (Index < Offset) Offset = Index;
				if (Index - Offset >= names.Length)
					Offset = Index - names.Length + 1;
				change = true;
			}

			if (Input.GetKeyDown(KeyCode.Z))
				UI.Remove();
			if (Input.GetKeyDown(KeyCode.S))
				UI.Remove();
			
			if (change) Refresh();
		}

		private void Refresh()
		{
			pocket.text = Pocket.Name;
			description.text = ((Item) Pocket.List[Index]).Description;

			for (int i = 0; i < pocketIcons.Length; i++)
			{
				if (i == pocketNum)
					pocketIcons[i].color = Color.black;
				else pocketIcons[i].color = Color.white;
			}

			for (int i = 0; i < names.Length; i++)
			{
				labels[i].gameObject.SetActive(i+Offset < Pocket.Count);
				if (i+Offset >= Pocket.Count) continue;

				Item item = (Item) Pocket.List[i+Offset];
				names[i].text = item.Name;
				if (item.Teach != null)
					names[i].text += " " + item.Teach.Name;
				nums[i].text = Pocket[item].ToString();
				float a = (i == Index-Offset) ? 0.5f : 0;
				labels[i].color = new Color(1,1,1,a);
				icons[i].sprite = Resources.Load<Sprite>(item.Icon);
			}

			for (int i = 0; i < Player.PartyLength; i++)
			{
				partyLabel[i].gameObject.SetActive(i < Player.Count);
				if (i >= Player.Count) continue;

				partySprite[i].sprite = Resources.Load<Sprite>(Player[i].Sprite);
				levels[i].text = "Nv " + Player[i].Level;
				partyNames[i].text = Player[i].Name;
				hp[i].text = Player[i].HP + "/" + Player[i].Stats["hp"];
			}
		}
	}
}