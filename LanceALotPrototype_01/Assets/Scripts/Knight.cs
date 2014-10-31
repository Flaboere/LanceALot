using UnityEngine;
using System.Collections;

public class Knight : MonoBehaviour {

	HingeJoint2D lanceJoint;

	void Start () {
		lanceJoint = GetComponent<HingeJoint2D>();
	}
	
	void Update ()
	{
		var lanceActive = BlackBoard.Read<bool>("Player", "ThumbSticksDown");
		lanceJoint.useMotor = lanceActive;
		
		if (lanceActive)
			lanceJoint.useLimits = false;
	}
}
