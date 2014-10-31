using UnityEngine;
using System.Collections;

public class Knight : MonoBehaviour {

	HingeJoint2D lanceJoint;

	// Use this for initialization
	void Start () {
		lanceJoint = GetComponent<HingeJoint2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space"))
		{
			/*JointAngleLimits2D lanceLimits = lanceJoint.limits;
			lanceLimits.min = -40;
			lanceJoint.limits = lanceLimits;*/
			lanceJoint.useLimits = false;
		}
	}
}
