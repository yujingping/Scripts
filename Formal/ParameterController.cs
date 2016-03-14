using UnityEngine;
using System.Collections;

public class ParameterController : MonoBehaviour 
{
	public float sensitivityThresold;

	private float defaultCompassDirection;
	private float joyStickValueX;
	private float joyStickValueY;

	void Awake()
	{
		Input.location.Start ();
		Input.compass.enabled = true;
		defaultCompassDirection = Input.compass.magneticHeading;
	}

	public Quaternion GetRealTimeViewDirection()
	{
		float xx, yy, zz;
		yy = Input.compass.magneticHeading - defaultCompassDirection;
		xx = -Input.acceleration.z * 90f;
		zz = -Input.acceleration.x * 90f;
		if (xx > 90f)
			xx = 90f;
		else if (xx < -90f)
			xx = -90f;
		return Quaternion.Euler (xx,yy,zz);
	}

	public Vector2 GetRealTimeJoyStickValue()
	{
		return new Vector2 (joyStickValueX,joyStickValueY);
	}

	void Update ()
	{

	}

	void OnEnable()
	{
		EasyJoystick.On_JoystickMove += EasyJoystick_On_JoystickMove;
		EasyJoystick.On_JoystickTouchUp += EasyJoystick_On_JoystickMoveEnd;
	}

	void EasyJoystick_On_JoystickMoveEnd (MovingJoystick move)
	{
		joyStickValueX = joyStickValueY = 0;
	}

	void OnDisable()
	{
		EasyJoystick.On_JoystickMove -= EasyJoystick_On_JoystickMove;
		joyStickValueX = joyStickValueY = 0;
	}

	void OnDestroy()
	{
		EasyJoystick.On_JoystickMove -= EasyJoystick_On_JoystickMove;
		joyStickValueX = joyStickValueY = 0;
	}

	void EasyJoystick_On_JoystickMove (MovingJoystick move)
	{
		joyStickValueX = move.joystickValue.x;
		joyStickValueY = move.joystickValue.y;
		if (Mathf.Abs(joyStickValueX) < sensitivityThresold)
			joyStickValueX = 0f;
		if (Mathf.Abs(joyStickValueY) < sensitivityThresold)
			joyStickValueY = 0f;
	}
}
