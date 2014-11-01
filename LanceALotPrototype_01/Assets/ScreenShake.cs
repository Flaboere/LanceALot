using UnityEngine;
using System.Collections;

public class ScreenShake : GameScript {

	private Vector2 startPosition;
	float time = 0.2f;
	float magnitude = 1f;

	// Use this for initialization
	void Start () {
		startPosition = transform.localPosition;
		base.Start ();
	}

	[RegisterMessage("Player", "HitGround")]
	void PlayerHitGround()
	{
		StopAllCoroutines();
		StartCoroutine("CoScreenshake");
	}

	[RegisterMessage("Player", "LanceHit")]
	void LanceHit()
	{
		StopAllCoroutines();
		StartCoroutine("CoScreenshake");
	}

	IEnumerator CoScreenshake (){

		while (time > 0) {
			time -= Time.deltaTime;
			transform.localPosition = Random.insideUnitCircle;
			yield return null;
		}
		transform.localPosition = startPosition;
	}
}
