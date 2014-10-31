using UnityEngine;
using System.Collections;

public class Horse : GameScript
{
	public AnimationCurve velocityCurve;

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
		float addition = velocityCurve.Evaluate (rigidbody2D.velocity.x);
		rigidbody2D.velocity += Vector2.right * addition;
	}

	[RegisterMessage ("Player", "HorseFly")]
	void Fly()
	{
		rigidbody2D.AddTorque(-30f);
	}
}
