using UnityEngine;
using System.Collections;

public class Knight : MonoBehaviour {

	HingeJoint2D lanceJoint;

	void Start () {
		lanceJoint = GetComponent<HingeJoint2D>();
	}
	
	void Update () {
		if (BlackBoard.Read<bool> ("Player", "ThumbSticksDown"))
		{
			lanceJoint.useLimits = false;
		}
	}
}
