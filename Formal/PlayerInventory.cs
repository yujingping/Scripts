using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour 
{
	public struct Info
	{
		public string name;
		public string introduction;

		public Info(string n,string i)
		{
			name = n;
			introduction = i;
		}
	};

	public List<Info> itemHashes = new List<Info>();

	void Awake()
	{
		itemHashes.Clear ();
	}

	public void InsertNewItemByNum(string n,string i)
	{
		Info temp = new Info (n, i);
		if (!itemHashes.Contains (temp))
			itemHashes.Add (temp);
	}
}
