using UnityEngine;
using System.Collections;

public class MenuScript : GameScript
{
	public AudioSource titleMusic;
	public Animation cameraAnimation;
	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RegisterMessage("Player", "Start")]
	public void StartGame ()
	{
		animation.Play("MenFadeOut");
		cameraAnimation.Play("StartGame");
		titleMusic.enabled = false;
		SoundController.Instance.CrowdNoise();
	}
}
