using UnityEngine;
using System.Collections;

public class LanceVisuals : MonoBehaviour {

	[Range (0, 2)]
	public float lengthRatio = 1; 
	[Range (1, 20)]
	public float tipAnimationFrequency = 10;
	public bool animateTip = true;
	private bool animationRunning = false;
	private float animationStartTime;
	private float lastAnimationState;


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (animateTip || animationRunning) {
			float currenttAnimationState = Mathf.Sin(Time.time*tipAnimationFrequency);
			if(!animateTip && currenttAnimationState*lastAnimationState < 0) {
				animationRunning = false;
				currenttAnimationState=0;
			} else {				
				animationRunning = true;
			}
			GetComponent<SpriteRenderer> ().material.SetFloat ("_tip", currenttAnimationState);
			lastAnimationState=currenttAnimationState;
		}

		float lanceLenghtScale = Mathf.Clamp (lengthRatio, 0, 1);
		float lanceBend = Mathf.Clamp ((1-lanceLenghtScale), 0, 1);
		GetComponent<SpriteRenderer> ().material.SetFloat ("_lance", -lanceBend);
		transform.localScale = new Vector3 (lanceLenghtScale, 1, 1);
	}

}
