using UnityEngine;
using System.Collections;

public class PlayerController : StateMachine
{
	public int controllerIndex;

	private XboxInputState state
	{
		get { return XboxInput.controllers[controllerIndex]; }
	}

	void Start()
	{
		base.Start();

		AddState("Idle", "Run");
		AddState ("Run", "Jump");
		AddState ("Jump");

		RequestState("Idle");
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
		if (state.ThumbStickLeft && state.ThumbStickRight)
			RequestState ("Jump");
	}

	void EnterJump()
	{
		Debug.Log("Done");
	}
}
