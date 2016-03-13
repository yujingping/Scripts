using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Target : int
{
	GameController = 0,
	UIController = 1,
	DialogueController = 2,
	PlayerFirst = 3,
	Camera = 4
};

public class EventManager : MonoBehaviour 
{
	private static EventManager instance;
	private MonoBehaviour[] targets = new MonoBehaviour[5];

	void Awake()
	{
		instance = GetComponent<EventManager> ();
		targets [(int)Target.GameController] = GameObject.FindGameObjectWithTag (Tags.GameController).GetComponent<GameController> ();
		targets [(int)Target.UIController] = GameObject.FindGameObjectWithTag (Tags.UIController).GetComponent<UIController> ();
		targets [(int)Target.PlayerFirst] = GameObject.FindGameObjectWithTag (Tags.Player).GetComponent<FirstPersonMoving> ();
	}

	public static void CallFunc(Target target, string methodName, List<object> param)
	{
		MonoBehaviour mb = instance.targets [(int)target];
		EventDelegate ed = new EventDelegate (mb, methodName);
		for (int i = 0; i < param.Count; i++)
			ed.parameters [i].value = param [i];
		ed.Execute ();
	}
}
