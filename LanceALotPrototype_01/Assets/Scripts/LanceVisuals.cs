using UnityEngine;
using System.Collections;

public class LanceVisuals : MonoBehaviour {

	private float lanceLengthRatio;
	private float? initialLanceLenght = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<SpriteRenderer> ().material.SetFloat ("_tip", Mathf.Sin(Time.time*10));
		if (initialLanceLenght != null) {
			GetComponent<SpriteRenderer> ().material.SetFloat ("_lance", lanceLengthRatio);
		}
	}

	public void UpdateLanceLength(float lanceLenght) {
		if (initialLanceLenght == null) {
			initialLanceLenght = lanceLenght;
		}
		lanceLengthRatio = lanceLenght / initialLanceLenght.Value;
	}
}
