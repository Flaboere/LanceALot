using UnityEngine;
using System.Collections;

public class MenuScript : GameScript {

	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RegisterMessage("Player", "Start")]
	void StartGame ()
	{
		gameObject.SetActive(false);

		SoundController.Instance.CrowdNoise();
	}
}
