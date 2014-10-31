using UnityEngine;

[RequireComponent(typeof(XboxInput))]
public class PlayerController : StateMachine
{
	static int controllerIndex = 0;
	private XboxInputState state { get { return XboxInput.controllers[controllerIndex]; } }

	enum ThumbStickTarget { Inner, Outer }
	ThumbStickTarget thumbStickTarget = ThumbStickTarget.Inner;
	static float thumbTargetValue = 0.9f;

	void Start()
	{
		base.Start();

		AddState ("Idle", "Run");
		AddState ("Run", "Vault");
		AddState ("Vault", "Fly");
		AddState ("Fly");

		RequestState ("Run");
	}

	void Update ()
	{
		base.Update();

		if (state.ADown)
		{
			BlackBoard.Clear ();
			Application.LoadLevel(1);
		}

		
		if(Input.GetKeyDown(KeyCode.D))
		{
			SendMessage ("Player", "AddHorseForce");
		}
		if(Input.GetKey(KeyCode.F))
		{
			BlackBoard.Write("Player", "ThumbSticksDown", true);
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
				if (state.ThumbStickLeftHorizontal > thumbTargetValue && state.ThumbStickRightHorizontal < -thumbTargetValue)
				{
					SendMessage ("Player", "AddHorseForce");
					thumbStickTarget = ThumbStickTarget.Outer;
				}
				break;

			case ThumbStickTarget.Outer:
				if (state.ThumbStickLeftHorizontal < -thumbTargetValue && state.ThumbStickRightHorizontal > thumbTargetValue)
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
