using UnityEngine;
using System.Collections;

public class Reminder : MonoBehaviour 
{
	public UILabel description;
	public UILabel title;

	private const string unReadString = "[[FFFF66]NEW[-]] ";
	private bool isRead;
	private int index;
	private string originalTitle;
	private ContentReader contentReader;

	public void Initialize(bool i, string t, string c,int ind)
	{
		description = transform.FindChild ("Tween").FindChild ("Label - Description").GetComponent<UILabel> ();
		title = transform.FindChild ("Label - Title").GetComponent<UILabel> ();
		contentReader = transform.parent.GetComponent<ContentReader> ();
		isRead = i;
		index = ind;
		originalTitle = t;
		description.text = c;
		if (!isRead)
			title.text = unReadString + originalTitle;
		else
			title.text = originalTitle;
	}

	public void Read()
	{
		if (isRead)
			return;
		isRead = true;
		title.text = originalTitle;
		contentReader.SetReadState (index);
	}
}
