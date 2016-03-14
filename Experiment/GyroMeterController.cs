using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GyroMeterController : MonoBehaviour 
{
	private Text text;

	void Awake()
	{
		text = GetComponent<Text> ();
		Input.gyro.enabled = true;
	}

	void Update()
	{
		text.text = " " + Input.gyro.userAcceleration.x + " " + Input.gyro.userAcceleration.y + " " + Input.gyro.userAcceleration.z;
		if (Input.gyro.userAcceleration.x >= 0.8f)
			text.color = new Color (1f, 0f, 0f);
		else if (Input.gyro.userAcceleration.y >= 0.8f)
			text.color = new Color (0f, 1f, 0f);
		else if (Input.gyro.userAcceleration.z >= 0.8f)
			text.color = new Color (0f, 0f, 1f);
		else
			text.color = new Color (1f, 1f, 1f);
	}
}
