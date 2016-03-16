using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using PixelCrushers.DialogueSystem;

public class ContentReader : MonoBehaviour
{
	public InfoSaver infoSaver;
	public GameObject reminderPrefab;
	public int totReminderNum;
	public int updateFreq;

	private List<ReminderLine> lines = new List<ReminderLine> ();
	private List<int> indices = new List<int> ();

	struct ReminderLine
	{
		public bool isRead;
		public string title;
		public string content;

		public ReminderLine(string i, string t, string c)
		{
			if(i == "1")
				isRead = true;
			else isRead = false;
			title = t;
			content = c;
		}
	}

	void Awake()
	{
		indices.Clear ();
		for (int i = 0; i < totReminderNum; i++)
		{
			bool result;
			string name = Consts.VariableName.luaContentState + i;
			if (PlayerPrefs.GetInt (name) == 1)
				result = true;
			else
				result = false;
			DialogueLua.SetVariable (name, result);
			if (!result)
				indices.Add (i);
		}
		ReadContent ();
	}
		
	public void SetReadState(int index)
	{
		PlayerPrefs.SetInt ("Reminder" + index, 1);
	}

	private float timer = 0f;

	void Update()
	{
		timer += Time.deltaTime;
		if (timer >= updateFreq)
		{
			UpdateReminders ();
		}
	}

	private void ReadContent()
	{
		string texts;
		texts = Resources.Load ("Reminders").ToString ();
		string[] liness = texts.Split ('\n');
		foreach (string line in liness)
		{
			string[] line_values = line.Split (',');
			ReminderLine line_entry = new ReminderLine (line_values [0], line_values [1], line_values [2]);
			lines.Add (line_entry);
		}
	}

	public void UpdateReminders()
	{
		for(int i = 0;i < indices.Count; i++)
		{
			timer = 0f;
			int x = indices [i];
			string name = Consts.VariableName.luaContentState + x;
			Debug.Log (name);
			if (DialogueLua.GetVariable (name).AsBool)
			{
				PlayerPrefs.SetInt (name, 1);
				DialogueLua.SetVariable (name, true);
				string title = lines [x].title;
				string content = lines [x].content;
				GameObject temp = NGUITools.AddChild (gameObject, reminderPrefab);
				temp.GetComponent<Reminder> ().Initialize (PlayerPrefs.GetInt("Reminder" + x) == 1, title, content, x);
				indices.Remove (x);
				i--;
				GetComponent<UITable> ().Reposition ();
			}
		}
	}
}