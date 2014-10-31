using UnityEngine;
using System.Collections;

public class Horse : GameScript
{
	public AnimationCurve velocityCurve;

	void Start () {
		base.Start();

		foreach (var component in GetComponentsInChildren<Rigidbody2D> ())
		{
			component.isKinematic = true;
		}
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
	}

	[RegisterMessage("Player", "LanceHit")]
	void LanceHit()
	{
		collider2D.enabled = false;

		foreach (var component in GetComponentsInChildren<Animator> ())
		{
			component.enabled = false;
		}

		foreach (var component in GetComponentsInChildren<ParticleSystem> ())
		{
			component.gameObject.SetActive(false);
		}

		foreach (var component in GetComponentsInChildren<Rigidbody2D> ())
		{
			component.isKinematic = false;
		}
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
		{
			if (joint.tag == "Release")
				joint.enabled = false;
		}

		foreach (var joint in GetComponentsInChildren<HingeJoint2D>())
		{
			if (joint.tag == "Release")
				joint.enabled = false;
		}
	}
}
