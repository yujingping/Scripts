using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestButton2 : MonoBehaviour 
{
	private Button but;

	void Awake()
	{
		but = GetComponent<Button> ();
		//but.onClick.AddListener (Camera.main.GetComponent<PhotoCamera> ().TakePhotograph);
	}
}
