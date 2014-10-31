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

	void OnCollisionEnter2D(Collision2D target)
	{
		Debug.Log("lolll");
		if (target.gameObject.tag == "Ground")
		{
			SendMessage("Player", "HitGround");
		}
	}
}
