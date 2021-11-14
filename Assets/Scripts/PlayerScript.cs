using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Pokemon.UI;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// Clase que controla el movimiento del jugador.
/// Utiliza el patrón Singleton, sólo existe una instancia de esta clase.
/// El objeto que lleve este script debe ser el jugador.
/// </summary>
public class PlayerScript : Command
{
	public float moveSpeed = 4f;
	public float runSpeed = 8f;

	private float direction = 0f;

	public static PlayerScript Instance { get; private set; }
	private void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(this.gameObject);
		else
		{
			Instance = this;
			DontDestroyOnLoad(Instance);
		}
	}

	public override IEnumerator Execute()
	{
		MovePlayer();

		if (axis != Vector2Int.zero)
		{
			Event.animator.SetBool("isRunning", Input.GetKey(KeyCode.S));
			var model = transform.Find("Model");
			float i = 180 * Mathf.Atan2(axis.y, axis.x) / Mathf.PI + 90;
			model.Rotate(0, direction - i, 0, Space.World);
			direction = i;
		}

		if (Input.GetKeyDown(KeyCode.Space))
			Season.Textures(DateTime.Now.Minute);
		
		
		if (Input.GetKey(KeyCode.Tab))
			Time.timeScale = 2;
		else Time.timeScale = 1;

		if (Input.GetKeyDown(KeyCode.Z))
			UI.Add("Pause Menu");
		
		yield return null;
	}

	void MovePlayer()
	{
		Event.speed = Input.GetKey(KeyCode.S) ? runSpeed : moveSpeed;
		var input = Vector3.zero;

		if (!collision(right * axis.x))
			input += right * axis.x;
		if (!collision(forward * axis.y))
			input += forward * axis.y;

		if (input - transform.Find("Model").forward == Vector3.zero)
		{
			Event.target = transform.position + input;
		}
	}

	bool collision(Vector3 dir)
	{
		if (dir == Vector3.zero) return false;
		Vector3 origin = Event.target;
		origin.y = transform.position.y + 0.5f;
		RaycastHit hitInfo = new RaycastHit();
		Vector3 size = new Vector3(0.45f,0.40f,0.45f);

		// Colisiones en el proximo tile
		var collisions = Physics.OverlapBox(transform.position + dir - Vector3.down, size);
		int l = 0;
		foreach (var c in collisions)
		{
			if (!c.isTrigger)
				if (c.gameObject.layer != 3)
					l++;
		}

		if (l > 0)   // Si hay alguna colision
		{
			Debug.Log(collisions[0].gameObject);
			// Comprobar si es una escalera
			if (Physics.Raycast(origin,dir,out hitInfo,1.5f))
			{
				float angle = Vector3.Angle(hitInfo.normal,-Vector3.down);
				Debug.Log(angle);
				return angle > Event.controller.slopeLimit;
			}
			return true;   // Si no es una escalera, es un muro
		}

		// Comprobar si es una escalera o un barranco
		if (Physics.Raycast(origin+dir*2+Vector3.down,-dir,out hitInfo,2f))
		{
			if (Physics.Raycast(origin + dir*2, Vector3.down, 0.8f))
				return false;

			float angle = Vector3.Angle(hitInfo.normal,-Vector3.down);
			return angle > Event.controller.slopeLimit;
		}

		// Caida al vacío
		return !Physics.Raycast(origin+dir,Vector3.down,3.0f);
	}

	public static Vector2Int axis
	{
		get
		{
			int l = Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
			int r = Input.GetKey(KeyCode.RightArrow) ? 1 : 0;

			int u = Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
			int d = Input.GetKey(KeyCode.DownArrow) ? -1 : 0;

			return new Vector2Int(l+r,u+d);
		}
	}

	Vector3 right { get => -transform.right; }
	Vector3 forward { get => -transform.forward; }

	public static void TakeTransparentScreenshot()
	{
		Camera cam = Camera.main;
		string savePath = string.Format("Assets/Screenshots/capture_{0}.png", DateTime.Now.Ticks);
		if (!Directory.Exists("Assets/Screenshots"))
			Directory.CreateDirectory("Assets/Screenshots");

		// Depending on your render pipeline, this may not work.
		var bak_cam_targetTexture = cam.targetTexture;
		var bak_cam_clearFlags = cam.clearFlags;
		var bak_RenderTexture_active = RenderTexture.active;

		var tex_transparent = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
		// Must use 24-bit depth buffer to be able to fill background.
		var render_texture = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
		var grab_area = new Rect(0, 0, Screen.width, Screen.height);

		RenderTexture.active = render_texture;
		cam.targetTexture = render_texture;
		cam.clearFlags = CameraClearFlags.SolidColor;

		// Simple: use a clear background
		cam.backgroundColor = Color.clear;
		cam.Render();
		tex_transparent.ReadPixels(grab_area, 0, 0);
		tex_transparent.Apply();

		// Encode the resulting output texture to a byte array then write to the file
		byte[] pngShot = ImageConversion.EncodeToPNG(tex_transparent);
		File.WriteAllBytes(savePath, pngShot);

		cam.clearFlags = bak_cam_clearFlags;
		cam.targetTexture = bak_cam_targetTexture;
		RenderTexture.active = bak_RenderTexture_active;
		RenderTexture.ReleaseTemporary(render_texture);
		Texture2D.Destroy(tex_transparent);
	}
}
