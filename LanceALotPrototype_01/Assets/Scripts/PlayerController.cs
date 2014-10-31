using UnityEngine;

public class PlayerController : StateMachine
{
	public int controllerIndex;
	private XboxInputState state { get { return XboxInput.controllers[controllerIndex]; } }

	enum ThumbStickTarget { Inner, Outer }
	ThumbStickTarget thumbStickTarget = ThumbStickTarget.Inner;
	float thumbTargetValue = 0.9f;
	public float speed;
	void Start()
	{
		base.Start();

		AddState ("Idle", "Run");
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
		switch (thumbStickTarget)
		{
			case ThumbStickTarget.Inner:
				if (state.ThumbStickLeftHorizontal > thumbTargetValue && state.ThumbStickRightHorizontal < -thumbTargetValue)
				{
					speed++;
					thumbStickTarget = ThumbStickTarget.Outer;
				}
				break;

			case ThumbStickTarget.Outer:
				if (state.ThumbStickLeftHorizontal < -thumbTargetValue && state.ThumbStickRightHorizontal > thumbTargetValue)
				{
					speed++;
					thumbStickTarget = ThumbStickTarget.Inner;
				}
				break;
		}

		if (state.ThumbStickLeft && state.ThumbStickRight)
			RequestState ("Jump");
	}

	void EnterJump()
	{
		Debug.Log("Done");
	}
}
