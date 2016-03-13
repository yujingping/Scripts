using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfoSaver : MonoBehaviour 
{
	public UIGrid backpackGrid;
	public float autoSaveFreq;

	private Vector3 playerPosition;
	private int equippedItemHash;
	private int num_inventory;
	private int roomNumber;

	private GameObject playerGameObject;
	private GameController gameController;
	private UIController uiController;
	private PlayerInventory inventory;

	void Awake()
	{
		MatchObjects ();
		//LoadGame ();
		num_inventory = PlayerPrefs.GetInt ("num_inventory");
		for(int i = 0;i < num_inventory;i++)
		{
			string name = PlayerPrefs.GetString ("ItemName" + i);
			string introduction = PlayerPrefs.GetString ("ItemIntroduction" + i);
			uiController.AddNewItem (new ItemInformation (name, introduction));
		}
	}

	void Start()
	{
		StartCoroutine (Synchronize());
	}

	void MatchObjects()
	{
		playerGameObject = GameObject.FindGameObjectWithTag (Tags.Player);
		gameController = GameObject.FindGameObjectWithTag (Tags.GameController).GetComponent<GameController> ();
		uiController = GameObject.FindGameObjectWithTag (Tags.UIController).GetComponent<UIController> ();
		inventory = gameController.GetComponent<PlayerInventory> ();
	}

	IEnumerator Synchronize()
	{
		while (true)
		{
			yield return new WaitForSeconds (autoSaveFreq);
			SaveGame ();
		}
	}

	public void SaveGame()
	{
		playerPosition = playerGameObject.transform.position;
		for (int i = 0; i < num_inventory; i++)
		{
			PlayerPrefs.DeleteKey ("ItemName" + i);
			PlayerPrefs.DeleteKey ("ItemIntroduction" + i);
		}
		num_inventory = inventory.itemHashes.Count;
		for (int i = 0; i < num_inventory; i++)
		{
			PlayerPrefs.SetString ("ItemName" + i, inventory.itemHashes [i].name);
			PlayerPrefs.SetString ("ItemIntrodction" + i, inventory.itemHashes[i].introduction);
		}
		if (gameController.EquippedItem == null)
			equippedItemHash = 0;
		else
		equippedItemHash = Animator.StringToHash (gameController.EquippedItem.name);
	}

	public void LoadGame()
	{
		playerGameObject.transform.position = playerPosition;
		for (int i = 0; i < num_inventory; i++)
		{
			string name = PlayerPrefs.GetString ("ItemName" + i);
			string introduction = PlayerPrefs.GetString ("ItemIntroduction" + i);
			GameObject item = uiController.AddNewItem (new ItemInformation (name, introduction));
			if (Animator.StringToHash (name) == equippedItemHash)
				gameController.SetEquippedItem (item);
		}
	}
}
