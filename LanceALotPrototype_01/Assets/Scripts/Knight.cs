using UnityEngine;
using System.Collections;

public class Knight : GameScript {

	HingeJoint2D lanceJoint;

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
	}

	[RegisterMessage("Player", "ReleaseLance")]
	void ReleaseLance()
	{
		Vector3 direction = BlackBoard.Read<Vector3>("Player", "LanceDirection");
		rigidbody2D.AddForce(-direction * 30);
	}
	
	void OnCollisionEnter2D(Collision2D target)
	{
		if (target.gameObject.tag == "Ground")
		{
			SendMessage("Player", "HitGround");
		}
	}
}
