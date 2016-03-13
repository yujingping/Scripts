using UnityEngine;
using System.Collections;

public class TestDialogueStarter : MonoBehaviour 
{
	public ConversationController controller;

	void Awake()
	{
		
	}

	void Start()
	{
		controller.StartConversation (gameObject);
	}
}
