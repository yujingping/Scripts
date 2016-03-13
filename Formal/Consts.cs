using UnityEngine;
using System.Collections;

public class Consts 
{
	public class QuestCondition // such named to evade the defined "QuestState" in the Dialogue System;
	{
		public const string Success = "success";
		public const string Active = "active";
		public const string Failed = "fail";
	}

	public class VariableName
	{
		public const string state = "State";
		public const string name = "Name";
		public const string luaContentState = "ContentAvailability";
	}

	public enum UISettings
	{
		ThirdPerson,
		FirstPerson,
		BackPack,
		Photoing,
		MainMenu,
		Reminder
	}
}
