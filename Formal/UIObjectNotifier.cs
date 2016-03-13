using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIObjectNotifier : MonoBehaviour 
{
	private Text itemTest;
	private Text objectText;

	void Awake()
	{
		objectText = transform.FindChild ("ObjectText").GetComponent<Text> ();
	}

	public void SetName(string o)
	{
		objectText.text = o;
	}
}
