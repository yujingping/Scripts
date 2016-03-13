using UnityEngine;
using System.Collections;

public class ThirdPersonMoving : MonoBehaviour 
{
	public Transform lookAtPos;

	private ParameterController parameterController;
	private Transform m_cameraTrans;
	private Transform m_playerTrans;
	private Transform m_transform;

	[SerializeField] private float rotateSmoothValue = 10f;
	[SerializeField] private float pivotOffsetY = 1.75f;
	[SerializeField] private float cameraMoveSpeed = 3f;
	[SerializeField] private float offsetCollisionUnit = 0.5f;
	[SerializeField] private Vector3 cameraOffset;

	void Awake()
	{
		m_transform = transform;
		m_cameraTrans = transform.FindChild ("PlayerCamera");
		m_playerTrans = transform.FindChild (Tags.Player);
		parameterController = GameObject.FindGameObjectWithTag (Tags.GameController).GetComponent<ParameterController> ();
	}

	void FixedUpdate()
	{
		Vector2 joystickInput = parameterController.GetRealTimeJoyStickValue ();
		VisualAngleChange ();
		float h = joystickInput.x;
		float v = joystickInput.y;
		GetComponent<PlayerMovement> ().MovementManage (h, v,false);
		CameraFollow ();
	}

	void VisualAngleChange()
	{
		Quaternion visualRotation = parameterController.GetRealTimeViewDirection ();
		visualRotation.x = visualRotation.z = 0f;
		transform.rotation = Quaternion.Slerp (transform.rotation,visualRotation,rotateSmoothValue * Time.deltaTime);
	}

	void CameraFollow()
	{
		Vector3 playerWorldPosition = m_playerTrans.position;
		m_transform.position = new Vector3 (playerWorldPosition.x,playerWorldPosition.y + pivotOffsetY,playerWorldPosition.z);
		m_playerTrans.localPosition = new Vector3 (0f,-pivotOffsetY,0f);
		Vector3 playerPosition = m_playerTrans.localPosition;
		m_cameraTrans.localPosition = Vector3.Lerp (m_cameraTrans.localPosition, new Vector3 (playerPosition.x, playerPosition.y, playerPosition.z) + cameraOffset, Time.deltaTime * cameraMoveSpeed);
		CameraRealPosUpdate ();
	}

	void CameraRealPosUpdate()
	{
		Vector3 direction = m_cameraTrans.position - lookAtPos.position;
		direction.Normalize ();
		RaycastHit hit;
		if (Physics.Linecast (lookAtPos.position, m_cameraTrans.position, out hit))
		{
			if (hit.collider.name != "PlayerCamera")
				m_cameraTrans.position = hit.point;// - offsetCollisionUnit * direction;
		}
	}
}
