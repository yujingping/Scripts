using UnityEngine;
using System.Collections;

public class PosAndRotLerp : MonoBehaviour 
{
	public Transform targetTransform;
	public float translationSpeed;
	public float rotatingSpeed;

	private static float positionThresold = 0.1f;
	private static float rotationThresold = 1.5f;

	public bool isTranslationDone;
	public bool isRotationDone;
	private Vector3 targetPosition;
	private Quaternion targetRotation;
	private bool isChangePos;
	private bool isChangeRot;

	void Start()
	{
		targetPosition = targetTransform.position;
		targetRotation = targetTransform.rotation;
	}

	public void SetParams(Transform t,float ts,float rs,bool iT,bool iR)
	{
		targetTransform = t;
		translationSpeed = ts;
		rotatingSpeed = rs;
		isChangePos = iT;
		isChangeRot = iR;
		isTranslationDone = !iT;
		isRotationDone = !iR;
	}

	void Update()
	{
		targetPosition = targetTransform.position;
		targetRotation = targetTransform.rotation;
		if (isChangePos) 
		{
			if (!isTranslationDone && Vector3.Distance (transform.position, targetPosition) <= positionThresold) 
			{
				transform.position = targetPosition;
				isTranslationDone = true;
			}
			else
				transform.position = Vector3.Slerp (transform.position,targetPosition,translationSpeed * Time.deltaTime);
		}
		float xx = Mathf.Abs (transform.rotation.eulerAngles.x - targetRotation.eulerAngles.x);
		float yy = Mathf.Abs (transform.rotation.eulerAngles.y - targetRotation.eulerAngles.y);
		float zz = Mathf.Abs (transform.rotation.eulerAngles.z - targetRotation.eulerAngles.z);
		if (isChangeRot) 
		{
			if (!isRotationDone && xx <= rotationThresold && yy <= rotationThresold && zz <= rotationThresold) 
			{
				transform.rotation = targetRotation;
				isRotationDone = true;
			}
			else
				transform.rotation = Quaternion.Slerp (transform.rotation,targetRotation,rotatingSpeed * Time.deltaTime);
		}
		if (isRotationDone && isTranslationDone)
			Destroy (GetComponent<PosAndRotLerp>(),0.01f);
	}
}
