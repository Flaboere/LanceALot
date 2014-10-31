using UnityEngine;
using System.Collections;

public class SoundController : GameScript {

	public static SoundController Instance;
	
	public SoundPlayer gallop;
	public SoundPlayer horseLaught1;
	public SoundPlayer horseLaught2;
	public SoundPlayer horseNoise;
	public SoundPlayer whoohoo;	
	public SoundPlayer excited_crowd;

	public void Awake()
	{
		Instance = GameObject.Find("SoundController").GetComponent<SoundController>();
	}

	void Start()
	{
		base.Start();
	}

	void Update()
	{
		if(released) return;
		var speed = BlackBoard.Read<float>("Horse","speed");
		if(speed<0.1)
			PlayGallop(false);
		else
			PlayGallop(true);
	}

	bool released;
	[RegisterMessage("Player", "ReleaseLance")]
	void ReleaseLance()
	{
		if(released)
			return;
		released = true;
		PlayGallop(false);
//		var r = (int)Random.Range(0,3);
//		if(r%3==0)
//			SoundController.Instance.HorseLaugth1();
//		else if(r%3==1)
//			SoundController.Instance.HorseLaugth2();
//		else
			SoundController.Instance.HorseNoise();

		Crowd();
		//Whoohoo();
	}

	[RegisterMessage("Player", "HitGround")]
	void PlayerHitGround()
	{
		var r = (int)Random.Range(0,2);
		if(r%2==0)
			SoundController.Instance.HorseLaugth1();
		else if(r%2==1)
			SoundController.Instance.HorseLaugth2();

		Crowd();
	}

	[RegisterMessage("Player", "AddHorseForce")]
	void AddHorseForce()
	{
		var xVelocity =  BlackBoard.Read<float>("Horse","speed");
		if(xVelocity>30) xVelocity = 30;
		var normalizedPitch = 0.5f + xVelocity/30;
		SoundController.Instance.SetGallopPitch(normalizedPitch);
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
	
	// Use this for initialization
	public  void HorseNoise()
	{
		Instance.horseNoise.PlayNormal();
	}

	
	// Use this for initialization
	public  void Crowd()
	{
		Instance.excited_crowd.PlayNormal();
	}

	
	// Use this for initialization
	public  void Whoohoo()
	{
		Instance.whoohoo.PlayNormal();
	}
}
