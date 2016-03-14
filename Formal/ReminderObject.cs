using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class ReminderObject : PickUpObject 
{
	public int index;

	public override void PhotoTaken()
	{
		PlayerPrefs.SetInt (Consts.VariableName.luaContentState + index, 1);
		DialogueLua.SetVariable (Consts.VariableName.luaContentState + index, true);
		gameController.AddItem (objectName,introduction);
		if(isExist || isReusable)
			Instantiate (pickUpEffect, transform.position, Quaternion.identity);
		if (isReusable)
			return;
		isExist = false;
		DeleteNotification ();
		GetComponent<Renderer> ().material = dissolveMat;
		StartCoroutine (Dissolve());
	}
}
