using UnityEngine;
using System.Collections;

public class Horse : GameScript
{

	public AnimationCurve velocityCurve;

	void Start () {
		base.Start();
	}
	
	[RegisterMessage("Player", "AddHorseForce")]
	void AddHorseForce()
	{
		float addition = velocityCurve.Evaluate (rigidbody2D.velocity.x);
		rigidbody2D.velocity += Vector2.right * addition;
	}

	[RegisterMessage ("Player", "HorseFly")]
	void Fly()
	{
		rigidbody2D.AddTorque(-30f);
	}
}
