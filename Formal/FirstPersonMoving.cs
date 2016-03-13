using UnityEngine;
using System.Collections;

public class FirstPersonMoving : MonoBehaviour 
{
	private Transform m_playerTrans;
	private ParameterController parameterController;
	private Transform m_cameraTrans;
	private Transform m_transform;

	[SerializeField] private float rotateSmoothValue = 10f;
	[SerializeField] private float pivotOffsetY = 1.75f;

	void Awake()
	{
		m_transform = transform;
		m_cameraTrans = transform.FindChild ("PlayerCamera");
		m_playerTrans = transform.FindChild ("Player");
		parameterController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ParameterController> ();
	}

	void FixedUpdate()
	{
		Vector2 joystickInput = parameterController.GetRealTimeJoyStickValue ();
		VisualAngleChange ();
		float h = joystickInput.x;
		float v = joystickInput.y;
		GetComponent<PlayerMovement> ().MovementManage (h, v, true);
		CameraFollow ();
	}

	void VisualAngleChange()
	{
		Quaternion visualRotation = parameterController.GetRealTimeViewDirection ();
		float yTemp = visualRotation.y;
		m_cameraTrans.rotation = Quaternion.Slerp (m_cameraTrans.rotation,visualRotation,rotateSmoothValue * Time.deltaTime);
		visualRotation.y = yTemp;
		visualRotation.x = visualRotation.z = 0f;
		m_transform.rotation = Quaternion.Slerp (m_transform.rotation,visualRotation,rotateSmoothValue * Time.deltaTime);
	}

	void CameraFollow()
	{
		Vector3 playerWorldPosition = m_playerTrans.position;
		m_transform.position = new Vector3 (playerWorldPosition.x, playerWorldPosition.y + pivotOffsetY, playerWorldPosition.z);
		m_playerTrans.localPosition = new Vector3 (0f,-pivotOffsetY,0f);
	}
}
