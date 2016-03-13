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
			if (result)
				indices.Add (i);
		}
		//for (int i = 0; i < totReminderNum; i++)
		//	PlayerPrefs.SetInt ("Reminder" + i, 0);
	}

	void Start()
	{
		string file = "/Reminders.txt";
		LoadReminders (file);
		DisplayReminders ();// this function serves as a testor only. Delete at any time you like.
		StartCoroutine (ContentUpdate ());
	}

	void Update()
	{
		
	}

	void LoadReminders(string fileName)
	{
		string file = Application.streamingAssetsPath + fileName;
		string line;
		StreamReader sr = new StreamReader (file);
		using (sr)
		{
			do
			{
				line = sr.ReadLine ();
				if(line != null)
				{
					string[] line_values = line.Split (',');
					ReminderLine line_entry = new ReminderLine (line_values[0], line_values[1], line_values[2]);
					lines.Add (line_entry);
				}
			}
			while(line != null);
			sr.Close();
		}
	}

	void DisplayReminders () // this function serves as a tester. Whenever finished testing simply remove this function!
	{
		for (int i = 0; i < 10; i++)
			DialogueLua.SetVariable (Consts.VariableName.luaContentState + i, true);
	}

	public void SetReadState(int index)
	{
		PlayerPrefs.SetInt ("Reminder" + index, 1);
	}

	IEnumerator ContentUpdate()//仔细研讨updateFreq的具体取值，防止坑爹状况的出现：玩家在拍完照的一瞬间推出了游戏，这导致玩家没有收集到这个手记但是在存档中这个手记的载体不会再存在。俗称：穿帮。
	{
		while (true)
		{
			for(int i = 0;i < indices.Count; i++)
			{
				int x = indices [i];
				string name = Consts.VariableName.luaContentState + x;
				if (DialogueLua.GetVariable (name).AsBool)
				{
					PlayerPrefs.SetInt (name, 1);
					string title = lines [x].title;
					string content = lines [x].content;
					GameObject temp = NGUITools.AddChild (gameObject, reminderPrefab);
					temp.GetComponent<Reminder> ().Initialize (PlayerPrefs.GetInt("Reminder" + x) == 1, title, content, x);
					indices.Remove (x);
					i--;
				}
			}
			yield return new WaitForSeconds (updateFreq);
		}
	}
}