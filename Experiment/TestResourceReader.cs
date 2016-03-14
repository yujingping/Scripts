using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestResourceReader : MonoBehaviour 
{
	void Awake()
	{
		GetComponent<Text> ().text = Resources.Load ("Test").ToString ();
	}
}
