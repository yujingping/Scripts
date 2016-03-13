using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class TestConversationStarter : MonoBehaviour 
{
	void Awake()
	{
		Debug.Log ("512315123");
		SetComponentEnabledOnDialogueEvent.SetComponentEnabledAction action = new SetComponentEnabledOnDialogueEvent.SetComponentEnabledAction ();
		action.target = GetComponent<BoxCollider> ();
		action.state = Toggle.False;
		GetComponent<SetComponentEnabledOnDialogueEvent> ().onEnd [0] = action;
	}
}
