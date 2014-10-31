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
		//rigidbody2D.AddTorque(-10f);
	}

	[RegisterMessage ("Player", "ReleaseLance")]
	void ReleaseLance()
	{
		Vector3 direction = BlackBoard.Read<Vector3> ("Player", "LanceDirection");
		rigidbody2D.AddForce (-direction * 30);
	}

	[RegisterMessage ("Player", "ReleaseParts")]
	void ReleaseParts()
	{
		foreach (var joint in GetComponents<HingeJoint2D>())
			joint.enabled = false;

		foreach (var joint in GetComponentsInChildren<HingeJoint2D> ())
			joint.enabled = false;
	}
}
