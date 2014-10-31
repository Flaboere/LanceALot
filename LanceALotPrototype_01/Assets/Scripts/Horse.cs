using UnityEngine;
using System.Collections;

public class Horse : GameScript
{
	public float force;

	void Start () {
		base.Start();
	}

	void Update()
	{
		BlackBoard.Write("Horse","speed", rigidbody2D.velocity.x);
	}

	[RegisterMessage("Player", "AddHorseForce")]
	void AddHorseForce()
	{
		rigidbody2D.AddForce (new Vector2 (force, 0));
	}

	[RegisterMessage ("Player", "HorseFly")]
	void Fly()
	{
		rigidbody2D.AddTorque(-30f);
	}
}
