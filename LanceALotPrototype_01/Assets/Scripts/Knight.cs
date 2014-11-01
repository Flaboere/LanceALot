using UnityEngine;
using System.Collections;

public class Knight : GameScript {

	HingeJoint2D lanceJoint;
	bool bounce = true;
	public float bounceForce;

	void Start () {
		base.Start();
		lanceJoint = GetComponent<HingeJoint2D>();
	}
	
	void Update ()
	{
		var lanceActive = BlackBoard.Read<bool>("Player", "ThumbSticksDown");
		lanceJoint.useMotor = lanceActive;
		
		if (lanceActive)
			lanceJoint.useLimits = false;

		if (bounce)
		{
			rigidbody2D.AddForce (Vector2.up * Random.value * bounceForce * Time.deltaTime);
		}
	}

	[RegisterMessage ("Player", "HitGround")]
	void PlayerHitGround()
	{
		lanceJoint.useMotor = false;
	}

	[RegisterMessage("Player", "ReleaseParts")]
	void ReleaseParts()
	{
		foreach (var joint in GetComponents<HingeJoint2D> ())
			joint.enabled = false;

		foreach (var joint in GetComponentsInChildren<HingeJoint2D> ())
		{
			if (joint.tag == "Release")
				joint.enabled = false;

			var limits = joint.limits;
			limits.min *= 2;
			limits.max *= 2;
			joint.limits = limits;
		}
	}

	[RegisterMessage("Player", "ReleaseLance")]
	void ReleaseLance()
	{
		Vector3 direction = BlackBoard.Read<Vector3>("Player", "LanceDirection");
		rigidbody2D.AddForce(-direction * 30);
	}
	
	void OnTriggerEnter2D(Collider2D target)
	{
		if (target.gameObject.tag == "Ground")
		{
			SendMessage("Player", "HitGround");
		}

		SendMessage ("Player", "Hit");
	}
}
