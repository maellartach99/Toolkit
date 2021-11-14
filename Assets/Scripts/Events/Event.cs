using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pokemon.UI;

public class Event : MonoBehaviour
{
	public Triggers Trigger;
	public CharacterController controller;
	public Animator animator;
	public bool Stop = true;
    public Command[] Commands;

	public Vector3 target { get; set; }
	public float speed { get; set; } = 4;
	protected bool MoveState = false;

	public static bool Wait = false;
	public static float gravity = 9.87f;

	public static IEnumerator Execute(bool Stop, Command[] Commands)
	{
		if (!Wait)
		{
			if (Stop)
				Wait = true;
			foreach (var command in Commands)
			{
				UI.StartCoroutine(command.StartCommand());
				yield return new WaitUntil( () => command.Finished );
				command.Finished = false;
			}
			if (Stop) Wait = false;
		}
		yield return null;
	}

	void Start()
	{
		target = transform.position;
		if (Trigger == Triggers.Start)
			UI.StartCoroutine(Execute(Stop,Commands));
	}
	void Update()
	{
		if (Trigger == Triggers.Accept)
		{
			if (Vector3.Distance(transform.position,Player.transform.position) < 2)
				if (Input.GetKeyDown(KeyCode.A))
					UI.StartCoroutine(Execute(Stop,Commands));
		}

		if (Trigger == Triggers.Update)
			UI.StartCoroutine(Execute(Stop,Commands));
	}
	void FixedUpdate()
	{
		if (controller != null)
			GridMove();
	}
	void OnTriggerEnter(Collider other)
	{
		if (Trigger == Triggers.Trigger)
			if (other.gameObject == Player)
				UI.StartCoroutine(Execute(Stop,Commands));
	}
	void OnCollisionEnter(Collision coll)
	{
		if (Trigger == Triggers.Collide)
			if (coll.collider.gameObject == Player)
				UI.StartCoroutine(Execute(Stop,Commands));
	}

	public void Move(Vector3 nuevo, bool relative = false)
    {
		target = nuevo;
		transform.position = nuevo;
    }

	private void GridMove()
	{
		float x = Mathf.Round(target.x);
		float y = target.y;
		float z = Mathf.Round(target.z);

		var hit = new RaycastHit();
		if (Physics.Raycast(target,Vector3.down,out hit))
		{
			if (Vector3.Angle(hit.normal,-Vector3.down) < controller.slopeLimit)
			{
				y = hit.point.y;
			}
		}

		target = new Vector3(x,y,z);

		if (animator != null)
			animator.SetBool("isMoving",MoveState);
		
		var (v1,v2) = (transform.position,target);
		v1.y = 0; v2.y = 0;
		float s = speed * Time.deltaTime;
		float d = Vector3.Distance(v1,v2);
		if (s/d < 1)
		{
			var newPos = Vector3.Lerp(transform.position, target, s/d);
			controller.Move(newPos-transform.position);
			MoveState = true;
		}
		else
		{
			controller.Move(target-transform.position);
			MoveState = false;
		}
	}

	public enum Triggers
	{
		None, Accept, Trigger, Collide, Start, Update
	}

	public static GameObject Player { get => PlayerScript.Instance.gameObject; }
	public static UserInterface UI { get => UserInterface.Instance; }
}
