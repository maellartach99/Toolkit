using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase para hacer cambiar las texturas del mapa en función de la estación del año.
/// </summary>
public class Season
{
	public static void Textures(int n)
	{
		var seasons = new string[] {"spring","summer","autumn","winter","snow"};
		Textures(seasons[n%5]);
	}

	public static void Textures(string season)
	{
		foreach (string nombre in names)
		{
			var material = Resources.Load<Material>("Maps/Materials/"+nombre);
			var textura = Resources.Load<Texture2D>("Maps/Textures/seasons/"+season+"/"+nombre);
			material.mainTexture = textura;
		}
	}

	private static string[] names = new string[] {
		"c12_flower", "C12_isi02", "C12_isi03", "C12_kadan02", "C12_ki01",
		"C12_mori_hasi", "C12_saien04", "C12_saien05", "C12_yama001", "C12_yama002",
		"C12_yama003", "C12_yama004", "gake1_1", "grass01ax", "grass01ax_lm1", "jimen3",
		"kemono01", "ki02ax", "ki02ax1", "ki02bx", "ki02bx1", "ki02c", "ki02c1", "ki02dx",
		"ki02dx1", "ki02ax_lm1", "ki02bx_lm1", "ki02c_lm1", "ki02dx_lm1", "ki03ax", "ki03bx",
		"ki03dx", "ki03ax_lm1", "ki03bx_lm1", "ki03dx_lm1", "kisetu_hana", "kisetu_yuki",
		"kusa_ec1", "kusa_ec1s", "kusa_ec2", "kusa_ec2s", "lambert77", "lav", "michi01a",
		"michi03a", "michi03b", "michi_hage", "michi_hage02", "mizutama01", "ochiba01c",
		"plant01", "plant01_1", "plant01_1_1", "plant01_1_2", "ue_grass00", "ue_grass01",
		"yamagrs01"
	};
}
