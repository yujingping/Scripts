using UnityEngine;
using System.Collections;

public class ItemInformation : MonoBehaviour
{
	public string itemName;
	public string introduction;

	public ItemInformation()
	{
		
	}

	public ItemInformation(string i,string j)
	{
		itemName = i;
		introduction = j;
	}
}
