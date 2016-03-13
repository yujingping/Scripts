using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIController : MonoBehaviour 
{
	public GameObject NGUIRoot;
	public GameObject UGUIRoot;
	public GameObject itemNotifier;
	public GameObject itemSprite;
	public UIGrid uiGrid;

	public float photoEffectSpeed;

	public GameObject photoButton;
	public GameObject changeButton;
	public GameObject joyStick;
	public GameObject reminder;
	public GameObject mainMenuButton;
	public GameObject backPackButton;
	//public GameObject cameraNotifier; // 相当于取景框的作用，采用完全静态的图片就可以了。等到加入的时候再说。Flag在此。

	void Awake()
	{
		SetSituation (Consts.UISettings.ThirdPerson);
	}

	public void SetSituation(Consts.UISettings type)
	{
		switch (type)
		{
		case Consts.UISettings.ThirdPerson:
			photoButton.SetActive (false);
			changeButton.SetActive (true);
			joyStick.SetActive (true);
			mainMenuButton.SetActive (true);
			backPackButton.SetActive (true);
			reminder.SetActive (false);
			break;
		case Consts.UISettings.FirstPerson:
			photoButton.SetActive (true);
			changeButton.SetActive (true);
			joyStick.SetActive (true);
			mainMenuButton.SetActive (true);
			backPackButton.SetActive (true);
			reminder.SetActive (false);
			break;
		case Consts.UISettings.BackPack:
			photoButton.SetActive (false);
			changeButton.SetActive (false);
			joyStick.SetActive (false);
			backPackButton.SetActive (true);
			reminder.SetActive (false);
			mainMenuButton.SetActive (false);
			break;
		case Consts.UISettings.Photoing:
			photoButton.SetActive (false);
			changeButton.SetActive (false);
			joyStick.SetActive (false);
			mainMenuButton.SetActive (false);
			backPackButton.SetActive (false);
			reminder.SetActive (false);
			break;
		case Consts.UISettings.MainMenu:
			photoButton.SetActive (false);
			changeButton.SetActive (false);
			joyStick.SetActive (false);
			mainMenuButton.SetActive (false);
			backPackButton.SetActive (false);
			reminder.SetActive (false);
			break;
		case Consts.UISettings.Reminder:
			photoButton.SetActive (false);
			changeButton.SetActive (false);
			joyStick.SetActive (false);
			mainMenuButton.SetActive (false);
			backPackButton.SetActive (false);
			reminder.SetActive (true);
			break;
		default :
			break;

		}
	}

	public void SetNGUIState(bool state)
	{
		GameObject camera = NGUIRoot.transform.FindChild ("Camera").gameObject;
		camera.SetActive (state);
	}

	public void SetUGUIState(bool state)
	{
		UGUIRoot.SetActive (state);
	}

	public IEnumerator NotifyItems(List<PhotoObject> photoObjects,bool type)
	{
		GameObject notifier = NGUITools.AddChild(NGUIRoot,itemNotifier);
		if(type)
			notifier.GetComponent<ItemNotifier> ().Initialize (photoObjects,ItemNotifier.NotifierType.NewItem);
		else notifier.GetComponent<ItemNotifier> ().Initialize (photoObjects,ItemNotifier.NotifierType.ItemInformation);
		while (notifier != null)
			yield return null;
	}

	public IEnumerator PhotoEffect()
	{
		RawImage rawImage = UGUIRoot.transform.FindChild ("PhotoEffectImage").GetComponent<RawImage> ();
		rawImage.color = new Color (1f,1f,1f,0.8f);
		Color targetColor = new Color (1f, 1f, 1f, 0f);
		while (rawImage.color.a >= 0.1f)
		{
			rawImage.color = Color.Lerp (rawImage.color,targetColor,Time.deltaTime * photoEffectSpeed);
			yield return null;
		}
		rawImage.color = targetColor;
		//SetNGUIState (true);
	}

	public GameObject AddNewItem(ItemInformation II)
	{
		BackpackItem BI;
		BI = NGUITools.AddChild (uiGrid.gameObject,itemSprite).GetComponent<BackpackItem> ();
		BI.itemName = II.itemName;
		BI.introduction = II.introduction;
		uiGrid.AddChild (BI.transform);
		return BI.gameObject;
	}
}
