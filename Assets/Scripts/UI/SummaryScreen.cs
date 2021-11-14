using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.UI
{
	public class SummaryScreen : SceneUI
	{
		private Transform[] screens;
		private Transform General;
		private Text title;
		private Text _name;
		private Text level;
		private Text item;
		private Image itemIcon;
		private Image[] icons;

		/****************************************************/
		private Text number;
		private Text species;
		private Text ot;
		private Text id;
		private Text exp;
		private Text nextlv;
		private Image t0, t1, t2;

		/****************************************************/
		private Image hpbar;
		private Vector2 xanchors;
		private Text ability;
		private Text description;
		private Text[] _stats;
		private Text[] stats;

		/****************************************************/
		private Text nature;
		private Text date;
		private Text location;
		private Text obtained;
		private Text personality;
		private Text flavor;

		/****************************************************/
		private Text[] names, pp;
		private Image[] types, move;

		/****************************************************/

		private int _index;
		public int Index
		{
			get { return _index; }
			set
			{
				int c = Game.Data.Player.Count;
				_index = (value + c) % c;
				if (General != null)
				{
					general();
					info();
					memo();
					skills();
					moves();
					PokemonView.Instance.Change(Game.Data.Player[_index]);
				}
			}
		}

		private int _screen;
		public int Screen
		{
			get { return _screen; }
			set
			{
				_screen = (value + titles.Length) % titles.Length;
				general();
				info();
				memo();
				skills();
				moves();
			}
		}

		private readonly string[] titles = {
			"Información","Notas","Estadísticas",
			"Movimientos","Cintas"
		};

		private readonly string[,] personalities = {	
			{"Le encanta comer.","A menudo se duerme.","Duerme mucho","Suele perder cosas.","Le gusta relajarse."},
			{"Orgulloso de su fuerza.","Le gusta revolverse.","A veces se enfada.","Le gusta luchar.","Tiene mal genio."},
			{"Cuerpo resistente.","Es buen fajador.","Muy persistente.","Muy resistente","Muy perseverante."},
			{"Le gusta correr.","Oído siempre alerta.","Impetuoso y bobo.","Es un poco payaso.","Huye rápido"},
			{"Extremadamente curioso.","Le gusta hacer travesuras.","Muy astuto.","A menudo está en Babia.","Muy melindroso."},
			{"Voluntarioso.","Es algo orgulloso.","Muy insolente.","Odia perder.","Un poco cabezota."}
		};

		// Start is called before the first frame update
		void Start()
		{
			General = transform.Find("General");

			_name = General.Find("name").GetComponent<Text>();
			level = General.Find("level").GetComponent<Text>();
			title = General.Find("title").GetComponent<Text>();
			item = General.Find("item").GetComponent<Text>();
			itemIcon = General.Find("itemIcon").GetComponent<Image>();

			var ic = transform.Find("icons");
			icons = new Image[5];
			screens = new Transform[5];
			for (int i = 0; i < 5; i++)
			{
				icons[i] = ic.Find(""+i).GetComponent<Image>();
				screens[i] = transform.Find(""+i);
			}
			
			number = screens[0].Find("number").GetComponent<Text>();
			species = screens[0].Find("species").GetComponent<Text>();
			ot = screens[0].Find("ot").GetComponent<Text>();
			id = screens[0].Find("id").GetComponent<Text>();
			exp = screens[0].Find("exp").GetComponent<Text>();
			nextlv = screens[0].Find("nextlv").GetComponent<Text>();
			t0 = screens[0].Find("t0").GetComponent<Image>();
			t1 = screens[0].Find("t1").GetComponent<Image>();
			t2 = screens[0].Find("t2").GetComponent<Image>();

			nature = screens[1].Find("nature").GetComponent<Text>();
			date = screens[1].Find("date").GetComponent<Text>();
			location = screens[1].Find("location").GetComponent<Text>();
			obtained = screens[1].Find("obtained").GetComponent<Text>();
			personality = screens[1].Find("personality").GetComponent<Text>();
			flavor = screens[1].Find("flavor").GetComponent<Text>();

			_stats = new Text[6];
			stats = new Text[6];
			var _s = screens[2].Find("_stats");
			var s = screens[2].Find("stats");
			for (int i = 0; i < 6; i++)
			{
				_stats[i] = _s.Find(""+i).GetComponent<Text>();
				stats[i] = s.Find(""+i).GetComponent<Text>();
			}
			ability = screens[2].Find("ability").GetComponent<Text>();
			description = screens[2].Find("description").GetComponent<Text>();
			hpbar = screens[2].Find("hpbar").GetComponent<Image>();
			var rect = hpbar.GetComponent<RectTransform>();
			xanchors = new Vector2(rect.anchorMin.x,rect.anchorMax.x);

			move = new Image[4];
			types = new Image[4];
			names = new Text[4];
			pp = new Text[4];
			for (int i = 0; i < 4; i++)
			{
				var trans = screens[3].Find(""+i);
				move[i] = trans.GetComponent<Image>();
				types[i] = trans.Find("type").GetComponent<Image>();
				names[i] = trans.Find("name").GetComponent<Text>();
				pp[i] = trans.Find("pp").GetComponent<Text>();
			}

			Screen = 0;
		}

		// Update is called once per frame
		public override void update()
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
				Index--;
			if (Input.GetKeyDown(KeyCode.DownArrow))
				Index++;
			if (Input.GetKeyDown(KeyCode.LeftArrow))
				Screen--;
			if (Input.GetKeyDown(KeyCode.RightArrow))
				Screen++;

			if (Input.GetKeyDown(KeyCode.A) && Screen == 3)
				UI.Add("Move Screen");
			if (Input.GetKeyDown(KeyCode.Z))
				UI.Remove();
			if (Input.GetKeyDown(KeyCode.S))
				UI.Remove();
		}

		private void general()
		{
			Pokemon pkm = Game.Data.Player[Index];
			_name.text = pkm.Name;
			level.text = "Nv " + pkm.Level;
			if (pkm.Item == null)
			{
				item.text = "Ninguno";
				itemIcon.gameObject.SetActive(false);
			}
			else
			{
				item.text = pkm.Item.Name;
				itemIcon.gameObject.SetActive(true);
				itemIcon.sprite = Resources.Load<Sprite>(pkm.Item.Icon);
			}
			
			for (int i = 0; i < titles.Length; i++)
			{
				screens[i].gameObject.SetActive(Screen == i);
				if (Screen == i)
					icons[i].color = new Color(1,1,1,1);
				else
					icons[i].color = new Color(1,1,1,0.5f);
			}
			title.text = titles[Screen];
		}

		private void info()
		{
			Pokemon pkm = Game.Data.Player[Index];
			number.text = pkm.Species.Num.ToString().PadLeft(3,'0');
			species.text = pkm.Species.Name;
			ot.text = Game.Data.Player.Name;
			id.text = pkm.PublicID.ToString().PadLeft(5,'0');
			exp.text = "" + pkm.Experience;
			nextlv.text = (pkm.Species.Experience.GetExp(pkm.Level+1) - pkm.Experience).ToString();

			bool types = pkm.Form.Types.Length == 1;
			t0.gameObject.SetActive(types);
			t1.gameObject.SetActive(!types);
			t2.gameObject.SetActive(!types);
			if (types)
			{
				t0.sprite = Resources.Load<Sprite>(pkm.Form.Types[0].Icon);
				t0.color = pkm.Form.Types[0].Color;
			}
			else
			{
				t1.sprite = Resources.Load<Sprite>(pkm.Form.Types[0].Icon);
				t2.sprite = Resources.Load<Sprite>(pkm.Form.Types[1].Icon);
				t1.color = pkm.Form.Types[0].Color;
				t2.color = pkm.Form.Types[1].Color;
			}
		}

		private void memo()
		{
			Pokemon pkm = Game.Data.Player[Index];
			var time = pkm.TimeReceived;
			string[] meses = {
				"Enero","Febrero","Marzo","Abril","Mayo","Junio",
				"Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre"
			};

			nature.text = "Naturaleza " + pkm.Nature.Name;
			date.text = time.Day + " de " + meses[time.Month-1] + " de " + time.Year;
			//location.text = 
			obtained.text = "Encontrado con Nv " + pkm.MeetLevel;

			var max = new KeyValuePair<Data.Stat,byte>(0,0);
			foreach (var pair in pkm.IV)
			{
				if (pair.Value > max.Value)
					max = pair;
			}
			personality.text = personalities[max.Key,max.Value%5];
			if (pkm.Nature.Neutral)
				flavor.text = "Come de todo";
			else
				flavor.text = "Le gusta lo " + pkm.Nature.Plus.Flavor;
		}

		private void skills()
		{
			Pokemon pkm = Game.Data.Player[Index];
			ability.text = pkm.Ability.Name;
			description.text = pkm.Ability.Description;

			float health = pkm.HP / (float) pkm.Stats["hp"];
			health = xanchors.x + health * (xanchors.y - xanchors.x);
			var rect = hpbar.GetComponent<RectTransform>();
			rect.anchorMax = new Vector2(health,rect.anchorMax.y);
			Color c = PartyScreen.bar(pkm);
			c.a = 0.69f;
			hpbar.color = c;

			stats[0].text = pkm.HP + "/" + pkm.Stats[0];
			for (int i = 1; i < 6; i++)
			{
				stats[i].text = pkm.Stats[i].ToString();
				_stats[i].color = Color.white;
			}
			if (!pkm.Nature.Neutral)
			{
				_stats[pkm.Nature.Plus].color = Color.red;
				_stats[pkm.Nature.Minus].color = Color.blue;
			}
		}

		private void moves()
		{
			Pokemon pkm = Game.Data.Player[Index];
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
		}
	}
}

