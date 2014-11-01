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
		magnitude = 0.05f;
		StopAllCoroutines();
		StartCoroutine("CoScreenshake");
	}

	[RegisterMessage("Player", "LanceHit")]
	void LanceHit()
	{
		magnitude = 0.25f;
		StopAllCoroutines();
		StartCoroutine("CoScreenshake");
	}

	[RegisterMessage("Player", "Hit")]
	void Hit()
	{
		magnitude = 0.05f;
		StopAllCoroutines();
		StartCoroutine("CoScreenshake");
	}

	IEnumerator CoScreenshake ()
	{

		float localTime = time;
		while (localTime > 0)
		{
			localTime -= Time.deltaTime;
			transform.localPosition = Random.insideUnitCircle * magnitude;
			yield return null;
		}
		transform.localPosition = startPosition;
	}
}
