using UnityEngine;
using System.Collections;

public class BackpackItem : ItemInformation 
{
	private UISprite sprite;

	void Awake()
	{
		sprite = GetComponent<UISprite> ();
		//sprite.spriteName = itemName;
	}
}
