using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class SaveDataEraser : MonoBehaviour 
{
	void Awake()
	{
		PlayerPrefs.DeleteAll();
	}
}
