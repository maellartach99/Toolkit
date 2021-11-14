using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que controla el movimiento de la cámara.
/// Utiliza el patrón Singleton, sólo existe una instancia de esta clase.
/// El objeto que lleve este script debe ser hijo del jugador
/// y tener a la cámara principal cómo hijo
/// </summary>
public class CameraControl : Command
{
	// Instancia del Singleton
	public static CameraControl Instance;
	
	public Camera Camera;
	public float rotateSpeed = 180;
	public float zoomSpeed = 50;

	private float angle = 0;
	private float distance = 0;

	private void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(this.gameObject);
		else Instance = this;
	}

	public override IEnumerator Execute()
	{
		Rotate();
		Zoom();
		yield return null;
	}

	
	void Rotate()
	{
		if (Input.GetKeyDown(KeyCode.Q)) angle -= 90;
		if (Input.GetKeyDown(KeyCode.E)) angle += 90;

		float delta = rotateSpeed * Time.deltaTime;
		if (delta > Mathf.Abs(angle)) delta = -angle;
		else delta *= (angle < 0) ? 1 : -1;

		transform.RotateAround(transform.position, Vector3.up, delta);
		angle += delta;
	}

	void Zoom()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
			distance = - Distance / 2;
		if (Input.GetKeyDown(KeyCode.LeftControl))
			distance = Distance;
		
		float delta = zoomSpeed * Time.deltaTime;
		if (delta > Mathf.Abs(distance)) delta = distance;
		else delta *= (distance > 0) ? 1 : -1;
		
		Distance += delta;
		distance -= delta;
	}

	float Distance
	{
		get => Camera.transform.localPosition.z;
		set => Camera.transform.localPosition = new Vector3(0,0,value);
	}
}
