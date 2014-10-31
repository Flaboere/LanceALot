using UnityEngine;
using System.Collections;

public class Horse : GameScript
{

	public float force;

	void Start () {
		base.Start();
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
