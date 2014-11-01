using UnityEngine;

[RequireComponent(typeof(XboxInput))]
public class PlayerController : StateMachine
{
	static int controllerIndex = 0;
	private XboxInputState state { get { return XboxInput.controllers[controllerIndex]; } }

	enum ThumbStickTarget { Inner, Outer }
	ThumbStickTarget thumbStickTarget = ThumbStickTarget.Inner;
	static float thumbTargetValue = 0.9f;

	public bool menuVisible = true;
	bool running;

	void Start()
	{
		base.Start();

		AddState ("Idle", "Run");
		AddState ("Run", "Vault");
		AddState ("Vault", "Fly");
		AddState ("Fly");

		//RequestState ("Run");
	}

	void Update ()
	{
		if(!menuVisible && !running)
		{
			running = true;
			Debug.Log("Run");
			RequestState ("Run");
			//SendMessage ("Player", "Start");
		}

		base.Update();

		if (state.ADown || Input.GetKeyDown(KeyCode.A))
		{
			//BlackBoard.Clear ();
			//Application.LoadLevel(1);
			Debug.Log("CloseMenu");
			SendMessage ("Player", "Start");
			menuVisible = false;
		}

		
		if(Input.GetKey(KeyCode.D))
		{
			SendMessage ("Player", "AddHorseForce");
		}
		if(Input.GetKey(KeyCode.F) && Time.frameCount%60==0)
		{
			BlackBoard.Write("Player", "ThumbSticksDown", true);
		}
		if(Input.GetKeyUp(KeyCode.F))
		{
			BlackBoard.Write ("Player", "ThumbSticksDown", false);
			SendMessage ("Player", "ReleaseLance");
			RequestState ("Fly");
		}
		if(Input.GetKeyDown(KeyCode.R))
		{
			Application.LoadLevel (Application.loadedLevelName);
			var menuScript = GameObject.Find("Menu").GetComponent<MenuScript>();
			menuScript.StartGame();
		}
	}

	void EnterIdle()
	{
		Debug.Log ("Idle");
	}

	void UpdateIdle()
	{
		if (state.ADown)
			RequestState("Run");
	}

	void EnterRun()
	{
		Debug.Log ("Run");
	}

	void UpdateRun()
	{

		switch (thumbStickTarget)
		{
			case ThumbStickTarget.Inner:
				if (state.ThumbStickLeftAngle > 360 - 45 
				&& state.ThumbStickLeftAngle < 0 + 45 
				&& state.ThumbStickRightAngle > 180 - 45 
				&& state.ThumbStickRightAngle < 180 + 45)
				{
					SendMessage ("Player", "AddHorseForce");
					thumbStickTarget = ThumbStickTarget.Outer;
				}
				break;

			case ThumbStickTarget.Outer:
				if (state.ThumbStickLeftAngle > 180 - 45 
			        && state.ThumbStickLeftAngle < 180 + 45 
			        && state.ThumbStickRightAngle > 360 - 45 
			        && state.ThumbStickRightAngle < 0 + 45)
				{
					SendMessage ("Player", "AddHorseForce");
					thumbStickTarget = ThumbStickTarget.Inner;
				}
				break;
		}

		if (state.ThumbStickLeft && state.ThumbStickRight)
		{
			BlackBoard.Write("Player", "ThumbSticksDown", true);
		}
	}


	[RegisterMessage("Player", "LanceHit")]
	void LanceHit ()
	{
		RequestState ("Vault");
	}

	[RegisterMessage("Player", "ReleaseLance")]
	void LanceReleased()
	{
		RequestState("Fly");
	}

	[RegisterMessage("Player", "HitGround")]
	void PlayerHitGround()
	{
		RequestState ("Fly");
	}

	void UpdateVault ()
	{
		if (!state.ThumbStickLeft && !state.ThumbStickRight)
		{
			BlackBoard.Write ("Player", "ThumbSticksDown", false);
			SendMessage ("Player", "ReleaseLance");
			RequestState ("Fly");
		}
	}

	void ExitVault()
	{
		BlackBoard.Write ("Player", "ThumbSticksDown", false);
	}
}
