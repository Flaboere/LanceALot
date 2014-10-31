using UnityEngine;
using System.Collections;

public class LanceVisuals : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<SpriteRenderer> ().material.SetFloat ("_tip", Mathf.Sin(Time.time*10));
	}
}
