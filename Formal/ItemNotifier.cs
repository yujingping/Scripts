using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemNotifier : MonoBehaviour 
{
	public enum NotifierType
	{
		ItemInformation,
		NewItem
	}

	public NotifierType notifierType;
	public UISprite itemSprite;
	public UILabel itemName;
	public UILabel itemIntroduction;
	public UIButton interactiveButton;
	public UIGrid uiGrid;
	public bool isEquipped;
	public List<ItemInformation> items = new List<ItemInformation>();

	private UILabel label_button;
	private GameController gameController;
	private int iterator;

	void Awake()
	{
		iterator = 0;
		interactiveButton.isEnabled = true;
		label_button = interactiveButton.transform.FindChild ("Label").GetComponent<UILabel> ();
		gameController = GameObject.FindGameObjectWithTag (Tags.GameController).GetComponent<GameController> ();
	}

	public void SetToItemInformation()
	{
		label_button.text = "Equip";
		if (isEquipped)
			interactiveButton.isEnabled = false;
		BindButtonEvent ();
		uiGrid.GetComponent<UICenterOnChild> ().onCenter = UpdateInformationOnChange;
	}

	void UpdateInformationOnChange(GameObject target)
	{
		ItemInformation item = target.GetComponent<ItemInformation> ();
		if (item == null)
			Debug.Log ("Not Found!");
		itemSprite.spriteName = item.itemName;
		itemName.text = item.itemName;
		itemIntroduction.text = item.introduction;
		if (!gameController.EquippedItem)
			return;
		if (Animator.StringToHash (item.itemName) == Animator.StringToHash(gameController.EquippedItem.GetComponent<ItemInformation> ().itemName))
			interactiveButton.isEnabled = false;
		else
			interactiveButton.isEnabled = true;
	}

	void SetToNewItem()
	{
		if(items.Count != 1)
			label_button.text = "Next";
		else label_button.text = "Back";
	}

	void BindButtonEvent()
	{
		interactiveButton.onClick.Clear ();
		if (notifierType == NotifierType.ItemInformation)
			interactiveButton.onClick.Add (new EventDelegate (GetComponent<ItemNotifier> (), "EquipItem"));
		else
			interactiveButton.onClick.Add (new EventDelegate (GetComponent<ItemNotifier> (), "NextItem"));
	}

	public void EquipItem()
	{
		Debug.Log ("Item Equipped!");
		gameController.SetEquippedItem(uiGrid.GetComponent<UICenterOnChild> ().centeredObject);
		interactiveButton.isEnabled = false;
	}

	public void NextItem()
	{
		iterator++;
		if (iterator == items.Count)
		{
			interactiveButton.onClick.Add (new EventDelegate(GetComponent<TweenScale>(),"PlayForward"));
			interactiveButton.onClick.Add (new EventDelegate(GetComponent<TweenPosition>(),"PlayForward"));
			return;
		}
		itemSprite.spriteName = items [iterator].itemName;
		itemName.text = items [iterator].itemName;
		itemIntroduction.text = items [iterator].introduction;
		if (iterator == items.Count - 1)
		{
			label_button.text = "Back";
		}
	}

	public void Initialize(List<PhotoObject> photoObjects,NotifierType type)
	{
		notifierType = type;
		items.Clear ();
		foreach (PhotoObject po in photoObjects)
			items.Add (new ItemInformation(po.objectName,po.introduction));
		if (notifierType == NotifierType.NewItem)
			SetToNewItem ();
		else
			SetToItemInformation ();
		BindButtonEvent ();
		itemSprite.spriteName = items [0].itemName;
		itemName.text = items [0].itemName;
		itemIntroduction.text = items [0].introduction;
		iterator = 0;
	}

	public void DestoryNotifier()
	{
		Destroy (gameObject);
	}
}
