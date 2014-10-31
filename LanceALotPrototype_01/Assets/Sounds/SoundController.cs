using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	public static SoundController Instance;
	
	public SoundPlayer gallop;
	public SoundPlayer horseLaught1;
	public SoundPlayer horseLaught2;


	public void Awake()
	{
		Instance = GameObject.Find("SoundController").GetComponent<SoundController>();
	}

	// Use this for initialization
	public  void PlayGallop(bool start)
	{
		if(start)
			Instance.gallop.PlayNormal();
		else
			Instance.gallop.StopPlaying();
	}

	// Use this for initialization
	public  void SetGallopPitch(float pitch)
	{
		Instance.gallop.SetPitch(pitch);
	}

	// Use this for initialization
	public  void HorseLaugth1()
	{
		Instance.horseLaught1.PlayNormal();
	}
	// Use this for initialization
	public  void HorseLaugth2()
	{
		Instance.horseLaught2.PlayNormal();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
