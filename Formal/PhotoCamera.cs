using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhotoCamera : MonoBehaviour 
{
	public float maxDetectionDistance;
	public float maxDerectionAngle;
	public float reloadTime = 1f;
	public float takePhotoWaitTime = 0.5f;

	private List<PhotoObject> photoObjects = new List<PhotoObject> ();
	private List<RealPicture> pictures = new List<RealPicture> ();
	private GameController gameController;
	private UIController uiController;
	private Camera _camera;
	private Transform m_camTrans;
	public bool allowToTake;
	private GameObject player;
	public bool allowToNotify;
	private GameObject interactableObjectToNotify;

	private enum ProcessType
	{
		Item,
		Interact,
		None
	}

	private ProcessType processType;

	void Awake()
	{
		allowToTake = false;
		allowToNotify = false;
		pictures.Clear ();
		photoObjects.Clear ();
		_camera = Camera.main;
		m_camTrans = _camera.transform;
		player = GameObject.FindGameObjectWithTag (Tags.Player);
		uiController = GameObject.FindGameObjectWithTag (Tags.UIController).GetComponent<UIController> ();
		gameController = GameObject.FindGameObjectWithTag (Tags.GameController).GetComponent<GameController> ();
	}

	void Update()
	{
		if (allowToNotify)
			NotifyPhotoObjects ();
	}

	void NotifyPhotoObjects()
	{
		if (!allowToNotify)
			return;
		if (FindItems ())
		{
			processType = ProcessType.Item;
			if (interactableObjectToNotify != null)
				interactableObjectToNotify.GetComponent<PhotoObject> ().DeleteNotification ();
		}
		else if (FindInteractItems ())
			processType = ProcessType.Interact;
		else processType = ProcessType.None;
	}
		
	public void TakePhotograph()
	{
		if (!allowToTake)
			return;
		else
			StartCoroutine (PhotoGraphProcess());
	}

	IEnumerator PhotoGraphProcess()
	{
		SetUIVisibility (false);
		DisableNotifications ();
		player.GetComponent<FirstPersonMoving> ().enabled = false;
		gameController.SetOutline (false);
		DisableNotifications ();
		yield return StartCoroutine (uiController.PhotoEffect());
		StartCoroutine (ScreenShot ());
		yield return StartCoroutine (DealPictures());
		if (processType == ProcessType.Item)
		{
			foreach (PhotoObject po in photoObjects)
				po.PhotoTaken ();
			yield return new WaitForSeconds(2);
			uiController.SetNGUIState (true);
			yield return StartCoroutine (uiController.NotifyItems (photoObjects, true));
		} 
		else if (processType == ProcessType.Interact)
		{
			uiController.SetNGUIState (true);
			InteractableObject io = interactableObjectToNotify.GetComponent<InteractableObject> ();
			if (io.CheckFitted ())
				io.PhotoTaken ();
			else
			{
				List<PhotoObject> objects = new List<PhotoObject> ();
				objects.Clear ();
				objects.Add (io.GetComponent<PhotoObject> ());
				yield return StartCoroutine (uiController.NotifyItems (objects,true));
			}
		}
		SetUIVisibility (true);
		photoObjects.Clear ();
		gameController.UpdateObjects ();
		player.GetComponent<FirstPersonMoving> ().enabled = true;
		gameController.SetOutline (true);
	}

	IEnumerator DealPictures()
	{
		pictures.Clear ();
		for(int i = 0, imax = gameController.Pictures.Count; i < imax; i++)
			if(gameController.Pictures[i] != null)
				pictures.Add(gameController.Pictures [i].GetComponent<RealPicture> ());
		foreach (RealPicture rp in pictures)
		{
			if (rp.CheckValidation (m_camTrans))
			{
				yield return StartCoroutine (rp.TakePhoto());
			}
		}
	}

	bool FindItems()
	{
		bool isItemFound = false;
		foreach (GameObject go in gameController.PickUpObjects)
		{
			if (go == null)
				continue;
			PhotoObject ggp = go.GetComponent<PhotoObject> ();
			if (!ggp.isExist)
				continue;
			Vector3 direction = go.transform.position - m_camTrans.position;
			RaycastHit hit;
			if (Physics.Raycast (new Ray (m_camTrans.position, direction), out hit, maxDetectionDistance))
			{
				if (hit.collider.gameObject != go || Vector3.Angle (direction, m_camTrans.forward) > maxDerectionAngle)
				{
					ggp.DeleteNotification ();
					photoObjects.Remove (ggp);
				}
				else
				{
					hit.collider.GetComponent<PhotoObject> ().NotifyObject (_camera.WorldToScreenPoint (hit.collider.transform.position));
					if (!photoObjects.Contains (ggp))
						photoObjects.Add (ggp);
					isItemFound = true;
				}
			}
		}
		return isItemFound;
	}

	bool FindInteractItems()
	{
		float minimumAngle = 361f;
		bool isItemFound = false;
		photoObjects.Clear ();
		Vector3 tempPosition = new Vector3 ();
		GameObject temp = null;
		foreach (GameObject go in gameController.InteractObjects)
		{
			if (go == null)
				continue;
			PhotoObject ggp = go.GetComponent<PhotoObject> ();
			if (!ggp.isExist)
				continue;
			Vector3 direction = go.transform.position - m_camTrans.position;
			RaycastHit hit;
			if (Physics.Raycast (new Ray (m_camTrans.position, direction), out hit, maxDetectionDistance))
			{
				float angle = Vector3.Angle (direction, m_camTrans.forward);
				if (angle > maxDerectionAngle)
					go.GetComponent<PhotoObject> ().DeleteNotification ();
				else if (hit.collider.GetComponent<PhotoObject> () != null && angle < minimumAngle)
				{
					minimumAngle = angle;
					temp = hit.collider.gameObject;
					tempPosition = hit.collider.transform.position;
					isItemFound = true;
				}
			}
		}
		if (isItemFound)
		{
			if(interactableObjectToNotify != null)
			interactableObjectToNotify.GetComponent<PhotoObject> ().DeleteNotification ();
			interactableObjectToNotify = temp;
			interactableObjectToNotify.GetComponent<PhotoObject> ().NotifyObject (_camera.WorldToScreenPoint (tempPosition));
		}
		return isItemFound;
	}

	void SetUIVisibility(bool state)
	{
		allowToTake = allowToNotify = state;
		uiController.SetNGUIState (state);
	}

	public void DisableNotifications()
	{
		foreach (PhotoObject po in photoObjects)
			po.DeleteNotification ();
		if (interactableObjectToNotify != null)
			interactableObjectToNotify.GetComponent<PhotoObject> ().DeleteNotification ();
	}

	IEnumerator ScreenShot()
	{
		bool tttt = false;
		Texture2D screenShot = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
		screenShot.ReadPixels (new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		screenShot.Apply ();
		byte[] bytes = screenShot.EncodeToPNG();
		string filename = Application.dataPath + "/Screenshot.png";  
		System.IO.File.WriteAllBytes(filename, bytes);  
		Debug.Log(string.Format("截屏了一张图片: {0}", filename));
		if(!tttt)
		yield return null;
	}
}
