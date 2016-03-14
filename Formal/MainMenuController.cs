using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.UnityGUI;

public class MainMenuController : MonoBehaviour 
{
	public GUISkin guiSkin;
	public QuestLogWindow questLogWindow;

	private bool isMenuOpen = false;
	private Rect windowRect = new Rect(0,0,500,500);
	private ScaledRect scaledRect = ScaledRect.FromOrigin (ScaledRectAlignment.MiddleCenter,ScaledValue.FromPixelValue(375), ScaledValue.FromPixelValue(400));
	private UIController uiController;

	private bool currentQuestState = false;

	void Awake()
	{
		uiController = GameObject.FindGameObjectWithTag (Tags.UIController).GetComponent<UIController> ();
	}

	void Start()
	{
		
	}

	void Update()
	{
		
	}

	void OnGUI()
	{
		if (isMenuOpen)
		{
			if (guiSkin != null)
				GUI.skin = guiSkin;
			windowRect = GUI.Window (0, windowRect, WindowFunction, "Menu");
		}
	}

	private void WindowFunction(int windowID)
	{
		if (GUI.Button (new Rect (10, 60, windowRect.width - 20, 48), "Quest Log"))
		{
			SetMenuStatus (false);
			OpenQuestLog ();
		}
		if (GUI.Button (new Rect (10, 110, windowRect.width - 20, 48), "Close Menu"))
		{
			SetMenuStatus (false);
			GameObject player = GameObject.FindGameObjectWithTag (Tags.Player);
			if (player.GetComponent<FirstPersonMoving> ().enabled)
				uiController.SetSituation (Consts.UISettings.FirstPerson);
			else if (player.GetComponent<ThirdPersonMoving> ().enabled)
				uiController.SetSituation (Consts.UISettings.ThirdPerson);
		}
		if (GUI.Button (new Rect (10, 160, windowRect.width - 20, 48), "Main Menu"))
		{
			
		}
		if (GUI.Button (new Rect (10, 210, windowRect.width - 20, 48), "Reminders"))
		{
			uiController.SetSituation (Consts.UISettings.Reminder);
			SetMenuStatus (false);
		}
	}

	public void CallMainMenu()
	{
		uiController.SetSituation (Consts.UISettings.MainMenu);
		if (!IsQuestLogOpen())
			SetMenuStatus (!isMenuOpen);
	}

	private void SetMenuStatus(bool status)
	{
		isMenuOpen = status;
		if (status)
		{
			windowRect = scaledRect.GetPixelRect ();
			//Time.timeScale = status ? 0 : 1;
		}
		else
		{
			
		}
	}

	private bool IsQuestLogOpen()
	{
		return (questLogWindow != null) && questLogWindow.IsOpen;
	}

	private void OpenQuestLog()
	{
		if ((questLogWindow != null) && !IsQuestLogOpen ())
		{
			questLogWindow.Open ();
			uiController.SetSituation (Consts.UISettings.MainMenu);
			StartCoroutine (QuestLogDetection());
		}
	}

	IEnumerator QuestLogDetection()
	{
		while(true)
		{
			bool realState = IsQuestLogOpen ();
			if (realState == false)
			{
				GameObject player = GameObject.FindGameObjectWithTag (Tags.Player);
				if (player.GetComponent<FirstPersonMoving> ().enabled)
					uiController.SetSituation (Consts.UISettings.FirstPerson);
				else if (player.GetComponent<ThirdPersonMoving> ().enabled)
					uiController.SetSituation (Consts.UISettings.ThirdPerson);
				break;
			}
			yield return null;
		}
	}
}
