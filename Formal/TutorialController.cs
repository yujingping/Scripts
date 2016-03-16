using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour 
{
	public enum Type
	{
		OnStart,
		OnTrigger,
		OnEvent
	};

	public GameObject tutorWindow;

	private UIController uiController;
	private GameObject nguiRoot;

	void Awake()
	{
		uiController = GameObject.FindGameObjectWithTag (Tags.UIController).GetComponent<UIController> ();
		nguiRoot = uiController.NGUIRoot;
		StartTutorial ();
	}

	public void StartTutorial()
	{
		StartCoroutine (test());
	}

	IEnumerator test()
	{
		yield return new WaitForSeconds (1f);
		GameObject tutor = NGUITools.AddChild (nguiRoot, tutorWindow);
		tutor.GetComponent<TutorSystem> ().StartTutoring ();
	}
}
