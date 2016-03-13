using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class ConversationController : MonoBehaviour 
{
	public DialogueSystemController dsc;

	private GameController gameController;
	private UIController uiController;
	private GameObject player;
	private int toggleNum;
	private bool currentState;

	void Awake()
	{
		gameController = GameObject.FindGameObjectWithTag (Tags.GameController).GetComponent<GameController> ();
		uiController = GameObject.FindGameObjectWithTag (Tags.UIController).GetComponent<UIController> ();
		player = GameObject.FindGameObjectWithTag (Tags.Player);
		currentState = false;
	}

	void Start()
	{
		StartCoroutine (ConversationDetect());
	}

	public void StartConversation(GameObject targetConversationTrigger)
	{
		targetConversationTrigger.gameObject.SendMessage ("OnUse",this.transform,SendMessageOptions.DontRequireReceiver);
	}

	IEnumerator ConversationDetect()
	{
		while (true)
		{
			if (DialogueManager.IsConversationActive != currentState)
			{
				currentState = !currentState;
				StateChange ();
			}
			yield return null;
		}
	}

	void StateChange()
	{
		player.GetComponent<ThirdPersonMoving> ().enabled = !currentState;
		uiController.SetNGUIState (!currentState);
		uiController.SetUGUIState (!currentState);
	}
}
