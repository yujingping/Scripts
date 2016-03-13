using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhotoObject : MonoBehaviour 
{
	public UIObjectNotifier uiObjectNotifier;

	public string objectName;
	public string introduction;

	[SerializeField] protected UIObjectNotifier refNotifier;
	protected GameController gameController;
	protected UIController uiController;
	protected bool isNotified;
	public bool isExist;

	public void NotifyObject(Vector2 screenPos)
	{
		if (!isExist)
		{
			DeleteNotification ();
			return;
		}
		if (!isNotified)
		{
			isNotified = true;
			refNotifier = Instantiate (uiObjectNotifier, new Vector3 (0f, 0f, 0f), Quaternion.identity) as UIObjectNotifier;
			refNotifier.transform.parent = uiController.UGUIRoot.transform;
			refNotifier.GetComponent<RectTransform> ().position = new Vector3 (screenPos.x,screenPos.y,0f);
			refNotifier.SetName (objectName);
		} 
		else
			refNotifier.GetComponent<RectTransform> ().position = new Vector3 (screenPos.x,screenPos.y,0f);
	}

	public void DeleteNotification()
	{
		if(refNotifier != null)
		Destroy (refNotifier.gameObject);
		isNotified = false;
	}

	public virtual void PhotoTaken()
	{
		
	}
}
