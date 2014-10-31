using UnityEngine;
using System.Collections;

public class SoundPlayer : MonoBehaviour {

	public bool play;
	public float pitch = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		audio.pitch = pitch;
		if(play && !audio.isPlaying)
		{
			play = false;
			audio.Play();
		}
	}

	public void PlaySlow()
	{
		pitch = 0.9f;
		play = true;
	}
	
	public void PlayNormal()
	{
		pitch = 1f;
		play = true;
	}

	
	public void PlayFast()
	{
		pitch = 1.2f;
		play = true;
	}

	public void StopPlaying()
	{
		audio.Stop();
	}

	public void SetPitch( float pitch)
	{
		this.pitch = pitch;
	}
}
