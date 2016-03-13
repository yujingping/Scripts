using UnityEngine;
using System.Collections;

public class BackPackButton : MonoBehaviour 
{
	public GameObject itemInformationNotifier;
	public GameObject backPackGrid;
	public float backPackWidth = 120f;

	private bool currentState; // false means disabled and true means enabled;
	private PhotoCamera _camera;
	private GameObject notifier;
	private bool isBackPackStillMoving;
	private UIController uiController;
	private GameController gameController;

	void Awake()
	{
		currentState = false;
		notifier = null;
		isBackPackStillMoving = false;
		_camera = Camera.main.GetComponent<PhotoCamera> ();
		uiController = GameObject.FindGameObjectWithTag (Tags.UIController).GetComponent<UIController> ();
		gameController = GameObject.FindGameObjectWithTag (Tags.GameController).GetComponent<GameController> ();
	}

	public void ChangeBackPackState()
	{
		if (isBackPackStillMoving)
			return;
		if (currentState)
			DisableBackPack ();
		else
			InvokeBackPack ();
		currentState = !currentState;
	}

	public void InvokeBackPack()
	{
		_camera.allowToTake = _camera.allowToNotify = false;
		uiController.SetUGUIState (false);
		isBackPackStillMoving = true;
		notifier = NGUITools.AddChild (transform.parent.gameObject,itemInformationNotifier);
		UIGrid grid = notifier.GetComponent<ItemNotifier> ().uiGrid = backPackGrid.transform.FindChild("Scroll View").transform.FindChild("Grid").GetComponent<UIGrid> ();
		notifier.GetComponent<ItemNotifier> ().SetToItemInformation ();
		if (gameController.EquippedItem)
			grid.GetComponent<UICenterOnChild> ().CenterOn (gameController.EquippedItem.transform);
		else
			grid.GetComponent<UICenterOnChild> ().CenterOn (notifier.GetComponent<ItemNotifier> ().uiGrid.GetChild(0));
		StartCoroutine(BackPackMove ());
		uiController.SetSituation (Consts.UISettings.BackPack);
	}

	public void DisableBackPack()
	{
		isBackPackStillMoving = true;
		uiController.SetUGUIState (true);
		_camera.allowToTake = _camera.allowToNotify = true;
		Destroy (notifier);
		StartCoroutine(BackPackMove ());
		GameObject player = GameObject.FindGameObjectWithTag (Tags.Player);
		if (player.GetComponent<FirstPersonMoving> ().enabled)
			uiController.SetSituation (Consts.UISettings.FirstPerson);
		if (player.GetComponent<ThirdPersonMoving> ().enabled)
			uiController.SetSituation (Consts.UISettings.ThirdPerson);
	}

	IEnumerator BackPackMove()
	{
		int targetY = currentState ? -120 : 0;
		while (Mathf.Abs(backPackGrid.transform.localPosition.y - targetY) >= 0.1f)
		{
			backPackGrid.transform.localPosition = Vector3.Slerp (backPackGrid.transform.localPosition,new Vector3(0f,targetY,0f),8 * Time.deltaTime);
			yield return null;
		}
		backPackGrid.transform.localPosition = new Vector3 (0f,targetY,0f);
		isBackPackStillMoving = false;
	}
}
