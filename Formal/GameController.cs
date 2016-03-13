using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{
	public float objectUpdateFreq = 3f;
	public float outlineStrength = 0.0067f;

	public GameObject EquippedItem {get{ return equippedItem;}}

	public List<GameObject> NPCs
	{
		get
		{
			return npcs;
		}
	}

	public List<GameObject> PickUpObjects 
	{
		get
		{
			return pickUpObjects;
		}
	}

	public List<GameObject> InteractObjects 
	{
		get
		{
			return interactObjects;
		}
	}

	public List<GameObject> Pictures
	{
		get
		{
			return pictures;
		}
	}

	private GameObject equippedItem;
	private List<GameObject> npcs = new List<GameObject> ();
	private List<GameObject> pictures = new List<GameObject> ();
	private List<GameObject> pickUpObjects = new List<GameObject> ();
	private List<GameObject> interactObjects = new List<GameObject> ();

	private PlayerInventory playerInvectory;
	private UIController uiController;

	void Awake()
	{
		playerInvectory = GetComponent<PlayerInventory> ();
		uiController = GameObject.FindGameObjectWithTag (Tags.UIController).GetComponent<UIController> ();
	}

	void Start()
	{
		StartCoroutine (SelectObjects());
	}

	IEnumerator SelectObjects()
	{
		while(true)
		{
			npcs = new List<GameObject> (GameObject.FindGameObjectsWithTag(Tags.NPC));
			pictures = new List<GameObject> (GameObject.FindGameObjectsWithTag (Tags.RealPic));
			pickUpObjects = new List<GameObject> (GameObject.FindGameObjectsWithTag (Tags.PickUpItem));
			interactObjects = new List<GameObject> (GameObject.FindGameObjectsWithTag (Tags.InteractableObject));
			InactiveObjectsCulling (npcs);
			InactiveObjectsCulling (pictures);
			InactiveObjectsCulling (pickUpObjects);
			InactiveObjectsCulling (interactObjects);
			yield return new WaitForSeconds (objectUpdateFreq);
		}
	}

	public void UpdateObjects()
	{
		StopCoroutine (SelectObjects());
		StartCoroutine (SelectObjects ());
	}

	public void SetEquippedItem(GameObject target)
	{
		equippedItem = target;
	}

	public void SetOutline(bool state)
	{
		float tarStrength = state ? outlineStrength : 0f;
		foreach (GameObject go in pickUpObjects)
		{
			go.GetComponent<Renderer> ().material.SetFloat ("_Outline",tarStrength);
		}
		foreach (GameObject go in interactObjects)
		{
			go.GetComponent<Renderer> ().material.SetFloat ("_Outline", tarStrength);
		}
	}

	public void AddItem(string name,string introduction)
	{
		playerInvectory.InsertNewItemByNum (name,introduction);
		uiController.AddNewItem (new ItemInformation(name,introduction));
	}

	private int x = 1;

	public void OddMethod(int q,GameObject go)
	{
		x++;
		//Debug.Log(x + q);
		//Debug.Log (go.name);
	}

	private void InactiveObjectsCulling(List<GameObject> toCull)
	{
		foreach (GameObject go in toCull)
		{
			if (!go.activeInHierarchy)
				toCull.Remove (go);
		}
	}
}
