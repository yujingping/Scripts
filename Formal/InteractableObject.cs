using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InteractableObject : PhotoObject 
{
	public string requiredItemName;

	void Awake()
	{
		isExist = true;
		isNotified = false;
		gameController = GameObject.FindGameObjectWithTag (Tags.GameController).GetComponent<GameController> ();
		uiController = GameObject.FindGameObjectWithTag (Tags.UIController).GetComponent<UIController> ();
		if (PlayerPrefs.GetInt (Consts.VariableName.interactableName + objectName) == 1)
			PhotoTaken ();
	}

	public bool CheckFitted()
	{
		if (gameController.EquippedItem == null || Animator.StringToHash(gameController.EquippedItem.GetComponent<ItemInformation> ().itemName) == Animator.StringToHash(requiredItemName))
			return true;
		else
			return false;
	}

	public override void PhotoTaken()
	{
		PlayerPrefs.SetInt (Consts.VariableName.interactableName + objectName, 1);
		//Debug.Log ("Sucessfully Interacted!");
	}
}
