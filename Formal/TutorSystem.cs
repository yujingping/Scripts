using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorSystem : MonoBehaviour 
{
	public string[] titles;
	public string[] contents;
	public Transform[] cameraPoses;
	public Vector2[] windowPoses;

	private UIController uiController;
	private Camera _camera;
	private GameObject playerGameObject;

	[SerializeField] private int m_numOfTutorials;
	[SerializeField] private float m_buttonWaitTime;
	[SerializeField] private int tutorID;

	private UIButton button;
	private UILabel title;
	private UILabel content;
	private UILabel label_button;

	private int m_currentIndex = -1;
	private bool m_playerVisualMode; // true means first person while false means third person

	private const float cameraTranslationSpeed = 5f;
	private const float cameraRotationSpeed = 4f;

	void Awake()
	{
		if (PlayerPrefs.GetInt ("Tutor" + tutorID) == 1)
		{
			//Destroy (gameObject);
		}
		button = transform.FindChild ("Button").GetComponent<UIButton> ();
		title = transform.FindChild ("Title").GetComponent<UILabel> ();
		content = transform.FindChild ("Content").GetComponent<UILabel> ();
		label_button = button.transform.FindChild ("Label").GetComponent<UILabel> ();
		_camera = Camera.main;
		playerGameObject = GameObject.FindGameObjectWithTag (Tags.Player);
		uiController = GameObject.FindGameObjectWithTag (Tags.UIController).GetComponent<UIController> ();
	}

	void Start()
	{
		label_button.text = "Next";
		button.onClick.Add (new EventDelegate (GetComponent<TutorSystem> (), "NextTutor"));
	}

	public void StartTutoring()
	{
		m_playerVisualMode = playerGameObject.GetComponent<FirstPersonMoving> ().enabled;
		if (m_playerVisualMode)
			playerGameObject.GetComponent<FirstPersonMoving> ().enabled = false;
		else
			playerGameObject.GetComponent<ThirdPersonMoving> ().enabled = false;
		uiController.SetSituation (Consts.UISettings.Tutoring);
		NextTutor ();
	}

	IEnumerator ButtonEnabling()
	{
		button.isEnabled = false;
		_camera.gameObject.AddComponent<PosAndRotLerp> ();
		_camera.gameObject.GetComponent<PosAndRotLerp> ().SetParams (cameraPoses[m_currentIndex],cameraTranslationSpeed,cameraRotationSpeed,true,true);
		while (_camera.gameObject.GetComponent<PosAndRotLerp> () != null)
			yield return null;
		button.isEnabled = true;
	}

	public void NextTutor()
	{
		if (m_currentIndex == m_numOfTutorials - 1)
		{
			button.onClick.Clear ();
			label_button.text = "Exit";
			button.onClick.Add (new EventDelegate (GetComponent<TutorSystem> (), "Exit"));
			return;
		}
		m_currentIndex++;
		StartCoroutine (ButtonEnabling ());
		title.text = titles [m_currentIndex];
		content.text = contents [m_currentIndex];
		transform.localPosition = windowPoses [m_currentIndex];

	}

	public void Exit()
	{
		PlayerPrefs.SetInt ("Tutor" + tutorID, 1);
		_camera.transform.rotation = Quaternion.identity;
		if (m_playerVisualMode)
		{
			uiController.SetSituation (Consts.UISettings.FirstPerson);
			playerGameObject.GetComponent<FirstPersonMoving> ().enabled = true;
		} 
		else
		{
			uiController.SetSituation (Consts.UISettings.ThirdPerson);
			playerGameObject.GetComponent<ThirdPersonMoving> ().enabled = true;
		}
		Destroy (gameObject);
	}
}
