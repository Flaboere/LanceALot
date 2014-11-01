using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class XboxInput : MonoBehaviour
{
    bool[] playerIndexSet;
    PlayerIndex[] playerIndex;
    GamePadState[] state;
    GamePadState[] prevState;

	private static XboxInputState[] s_DJHeroInputStates;
	public static XboxInputState[] controllers
	{
		get
		{
			if (s_DJHeroInputStates == null)
			{
				s_DJHeroInputStates = new XboxInputState[4];
				for (int i=0; i<s_DJHeroInputStates.Length; i++)
					s_DJHeroInputStates[i] = new XboxInputState();

			}
			return s_DJHeroInputStates;
		}
	}

    void Start()
    {
        playerIndexSet = new bool[4];
        playerIndex = new PlayerIndex[4];
        state = new GamePadState[4];
        prevState = new GamePadState[4];

    }

    void Update()
    {
		for (int i = 0; i < 4; ++i)
        {
            if (!playerIndexSet[i] || !prevState[i].IsConnected)
            {
				try{
	                PlayerIndex testPlayerIndex = (PlayerIndex)i;
	                GamePadState testState = GamePad.GetState(testPlayerIndex);
				
					if (testState.IsConnected)
	                {
	                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
						playerIndex[i] = testPlayerIndex;
	                    playerIndexSet[i] = true;
	                }
				}
				catch(System.Exception e)
				{
					Debug.LogException(e);
					enabled = false;
					return;
				}
            }
        }

        for (int i = 0; i < 4; i++)
        {
            state[i] = GamePad.GetState(playerIndex[i], GamePadDeadZone.None);

            if (playerIndexSet[i] && prevState[i].IsConnected)
            {
                UpdateState(state[i], prevState[i], i);
            }

            prevState[i] = state[i];
        }
    }

	void UpdateState(GamePadState gamePadState, GamePadState previousState, int index)
	{
		XboxInputState inputState = controllers[index];

		inputState.ThumbStickLeftVertical = ApplyDeadZoneAndSensitivity(gamePadState.ThumbSticks.Left.Y, inputState.ThumbStickDeadZone, 1);
		inputState.ThumbStickLeftHorizontal = ApplyDeadZoneAndSensitivity (gamePadState.ThumbSticks.Left.X, inputState.ThumbStickDeadZone, 1);

		inputState.ThumbStickLeftAngle = Mathf.Atan2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y)*Mathf.Rad2Deg;

		inputState.ThumbStickRightVertical = ApplyDeadZoneAndSensitivity (gamePadState.ThumbSticks.Right.Y, inputState.ThumbStickDeadZone, 1);
		inputState.ThumbStickRightHorizontal = ApplyDeadZoneAndSensitivity (gamePadState.ThumbSticks.Right.X, inputState.ThumbStickDeadZone, 1);

		inputState.ThumbStickRightAngle = Mathf.Atan2(gamePadState.ThumbSticks.Right.X, gamePadState.ThumbSticks.Right.Y)*Mathf.Rad2Deg;

		inputState.TriggerLeft = ApplyDeadZoneAndSensitivity (gamePadState.Triggers.Left, inputState.ThumbStickDeadZone, 1);
		inputState.TriggerRight= ApplyDeadZoneAndSensitivity (gamePadState.Triggers.Right, inputState.ThumbStickDeadZone, 1);

		inputState.Red = gamePadState.Buttons.B == ButtonState.Pressed;
		inputState.Green = gamePadState.Buttons.A == ButtonState.Pressed;
		inputState.Blue = gamePadState.Buttons.X == ButtonState.Pressed;
		inputState.YeaBoiee = gamePadState.Buttons.Y == ButtonState.Pressed;
		inputState.Start = gamePadState.Buttons.Start == ButtonState.Pressed;
		inputState.Back = gamePadState.Buttons.Back == ButtonState.Pressed;
		inputState.DPadUp = gamePadState.DPad.Up == ButtonState.Pressed;
		inputState.DPadRight = gamePadState.DPad.Right == ButtonState.Pressed;
		inputState.DPadDown = gamePadState.DPad.Down == ButtonState.Pressed;
		inputState.DPadLeft = gamePadState.DPad.Left == ButtonState.Pressed;
		inputState.ThumbStickLeft= gamePadState.Buttons.LeftStick == ButtonState.Pressed;
		inputState.ThumbStickRight = gamePadState.Buttons.RightStick == ButtonState.Pressed;
		
		inputState.BDown = gamePadState.Buttons.B == ButtonState.Pressed && previousState.Buttons.B == ButtonState.Released;
		inputState.ADown = gamePadState.Buttons.A == ButtonState.Pressed && previousState.Buttons.A == ButtonState.Released;
		inputState.XDown = gamePadState.Buttons.X == ButtonState.Pressed && previousState.Buttons.X == ButtonState.Released;
		inputState.YDown = gamePadState.Buttons.Y == ButtonState.Pressed && previousState.Buttons.Y == ButtonState.Released;
		inputState.StartDown = gamePadState.Buttons.Start == ButtonState.Pressed && previousState.Buttons.Start == ButtonState.Released;
		inputState.BackDown = gamePadState.Buttons.Back == ButtonState.Pressed && previousState.Buttons.Back == ButtonState.Released;
		inputState.DPadUpDown = gamePadState.DPad.Up == ButtonState.Pressed && previousState.DPad.Up == ButtonState.Released;
		inputState.DPadRightDown = gamePadState.DPad.Right == ButtonState.Pressed && previousState.DPad.Right == ButtonState.Released;
		inputState.DPadDownDown = gamePadState.DPad.Down == ButtonState.Pressed && previousState.DPad.Down == ButtonState.Released;
		inputState.DPadLeftDown = gamePadState.DPad.Left == ButtonState.Pressed && previousState.DPad.Left == ButtonState.Released;
		inputState.ThumbStickLeftDown = gamePadState.Buttons.LeftStick == ButtonState.Pressed && previousState.Buttons.LeftStick == ButtonState.Released; ;
		inputState.ThumbStickRightDown = gamePadState.Buttons.RightStick == ButtonState.Pressed && previousState.Buttons.RightStick == ButtonState.Released; ;

		inputState.BUp = gamePadState.Buttons.B == ButtonState.Released && previousState.Buttons.B == ButtonState.Pressed;
		inputState.AUp = gamePadState.Buttons.A == ButtonState.Released && previousState.Buttons.A == ButtonState.Pressed;
		inputState.XUp = gamePadState.Buttons.X == ButtonState.Released && previousState.Buttons.X == ButtonState.Pressed;
		inputState.YUp = gamePadState.Buttons.Y == ButtonState.Released && previousState.Buttons.Y == ButtonState.Pressed;
		inputState.StartUp = gamePadState.Buttons.Start == ButtonState.Released && previousState.Buttons.Start == ButtonState.Pressed;
		inputState.BackUp = gamePadState.Buttons.Back == ButtonState.Released && previousState.Buttons.Back == ButtonState.Pressed;
		inputState.DPadUpDown = gamePadState.DPad.Up == ButtonState.Released && previousState.DPad.Up == ButtonState.Pressed;
		inputState.DPadRightDown = gamePadState.DPad.Right == ButtonState.Released && previousState.DPad.Right == ButtonState.Pressed;
		inputState.DPadDownDown = gamePadState.DPad.Down == ButtonState.Released && previousState.DPad.Down == ButtonState.Pressed;
		inputState.DPadLeftDown = gamePadState.DPad.Left == ButtonState.Released && previousState.DPad.Left == ButtonState.Pressed;
		inputState.ThumbStickLeftUp = gamePadState.Buttons.LeftStick == ButtonState.Released && previousState.Buttons.LeftStick == ButtonState.Pressed; ;
		inputState.ThumbStickRightUp = gamePadState.Buttons.RightStick == ButtonState.Released && previousState.Buttons.RightStick == ButtonState.Pressed; ;
	}

	private float ApplyDeadZoneAndSensitivity (float value, float deadZone, float sensitivity)
	{
		float result = value * sensitivity;
		if (Mathf.Abs (result) < deadZone && deadZone > 0f)
			result = 0f;

		return result;
	}

	void LateUpdate ()
	{
		for (int i = 0; i < controllers.Length; i++)
		{
			controllers[i].BDown = false;
			controllers[i].ADown = false;
			controllers[i].XDown = false;
			controllers[i].YDown = false;
			controllers[i].StartDown = false;
			controllers[i].BackDown = false;
			controllers[i].DPadUpDown = false;
			controllers[i].DPadRightDown = false;
			controllers[i].DPadDownDown = false;
			controllers[i].DPadLeftDown = false;

			controllers[i].BUp = false;
			controllers[i].AUp = false;
			controllers[i].XUp = false;
			controllers[i].YUp = false;
			controllers[i].StartUp = false;
			controllers[i].BackUp = false;
			controllers[i].DPadUpUp = false;
			controllers[i].DPadRightUp = false;
			controllers[i].DPadDownUp = false;
			controllers[i].DPadLeftUp = false;
		}
	}
}

public class XboxInputState
{
	public bool Red;
	public bool Green;
	public bool Blue;
	public bool YeaBoiee;
	public bool Start;
	public bool Back;
	public bool DPadUp;
	public bool DPadRight;
	public bool DPadDown;
	public bool DPadLeft;
	public bool ThumbStickLeft;
	public bool ThumbStickRight;

	public bool BDown;
	public bool ADown;
	public bool XDown;
	public bool YDown;
	public bool StartDown;
	public bool BackDown;
	public bool DPadUpDown;
	public bool DPadRightDown;
	public bool DPadDownDown;
	public bool DPadLeftDown;
	public bool ThumbStickLeftDown;
	public bool ThumbStickRightDown;

	public bool BUp;
	public bool AUp;
	public bool XUp;
	public bool YUp;
	public bool StartUp;
	public bool BackUp;
	public bool DPadUpUp;
	public bool DPadRightUp;
	public bool DPadDownUp;
	public bool DPadLeftUp;
	public bool ThumbStickLeftUp;
	public bool ThumbStickRightUp;

	public float TriggerLeft;
	public float TriggerRight;

	public float ThumbStickLeftVertical;
	public float ThumbStickLeftHorizontal;
	public float ThumbStickLeftAngle;

	public float ThumbStickRightVertical;
	public float ThumbStickRightHorizontal;
	public float ThumbStickRightAngle;

	public float ThumbStickDeadZone = 0.05f;
}
