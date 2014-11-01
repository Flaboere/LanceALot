using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

	private Vector2 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			StopAllCoroutines();
			StartCoroutine("CoScreenshake");
		}
		transform.localPosition = startPosition;
	}

	IEnumerator CoScreenshake (){
		var time = 0.2f;
		var magnitude = 1f;

		while (time > 0) {
			time -= Time.deltaTime;
			transform.localPosition = Random.insideUnitCircle;
			yield return null;
		}
	}
}
