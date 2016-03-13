using UnityEngine;
using System.Collections;

public class VisualAngleSwitcher : MonoBehaviour 
{
	public Transform thirdPersonTrans;
	public Transform firstPersonTrans;
	public float cameraTranslationSpeed;
	public float cameraRotationSpeed;
	public bool currentVisualAngle; // true means first person while false means third person.

	private Camera _camera;
	private GameController gameController;
	private UIController uiController;

	private bool isMoving;

	void Awake()
	{
		isMoving = false;
		currentVisualAngle = false;
		_camera = transform.FindChild ("PlayerCamera").GetComponent<Camera>();
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		uiController = GameObject.FindGameObjectWithTag (Tags.UIController).GetComponent<UIController> ();
		GetComponent<ThirdPersonMoving> ().enabled = true;
		GetComponent<FirstPersonMoving> ().enabled = false;
	}

	public void SwitchVisualAngle()
	{
		if (_camera.gameObject.GetComponent<PosAndRotLerp> () != null || isMoving)
			return;
		if (currentVisualAngle)
			SwitchToThirdPerson ();
		else
			SwitchToFirstPerson ();
	}

	void SwitchToFirstPerson()
	{
		isMoving = true;
		GetComponent<ThirdPersonMoving> ().enabled = false;
		transform.FindChild ("Player").FindChild("char_ethan_body").GetComponent<SkinnedMeshRenderer>().enabled = false;
		_camera.gameObject.AddComponent<PosAndRotLerp> ();
		_camera.gameObject.GetComponent<PosAndRotLerp> ().SetParams (firstPersonTrans,cameraTranslationSpeed,cameraRotationSpeed,true,true);
		StartCoroutine (MoveOnChanging());
		gameController.SetOutline (true);
		GetComponent<NPCNotifier> ().SetNotifyState (false);
		_camera.GetComponent<PhotoCamera> ().allowToNotify = true;
		_camera.GetComponent<PhotoCamera> ().allowToTake = true;
		uiController.SetSituation (Consts.UISettings.FirstPerson);
	}

	void SwitchToThirdPerson()
	{
		isMoving = true;
		GetComponent<FirstPersonMoving> ().enabled = false;
		transform.FindChild ("Player").FindChild("char_ethan_body").GetComponent<SkinnedMeshRenderer>().enabled = true;
		_camera.gameObject.AddComponent<PosAndRotLerp> ();
		_camera.gameObject.GetComponent<PosAndRotLerp> ().SetParams (thirdPersonTrans,cameraTranslationSpeed,cameraRotationSpeed,true,true);
		StartCoroutine (MoveOnChanging());
		gameController.SetOutline (false);

		GetComponent<NPCNotifier> ().SetNotifyState (true);
		_camera.GetComponent<PhotoCamera> ().allowToTake = false;
		_camera.GetComponent<PhotoCamera> ().allowToNotify = false;
		_camera.GetComponent<PhotoCamera> ().DisableNotifications ();
		uiController.SetSituation (Consts.UISettings.ThirdPerson);
	}

	IEnumerator MoveOnChanging()
	{
		while (_camera.gameObject.GetComponent<PosAndRotLerp> () != null)
			yield return null;
		if (currentVisualAngle)
		{
			GetComponent<ThirdPersonMoving> ().enabled = true;
			_camera.GetComponent<PhotoCamera> ().allowToTake = false;
		}
		else
		{
			GetComponent<FirstPersonMoving> ().enabled = true;
			_camera.GetComponent<PhotoCamera> ().allowToTake = true;
		}
		currentVisualAngle = !currentVisualAngle;
		isMoving = false;
	}
}
