using UnityEngine;
using System.Collections;

public class PickUpObject : PhotoObject 
{
	public Material dissolveMat;
	public static float dissolvingSpeed = 0.4f;
	public bool isReusable;
	public GameObject pickUpEffect;

	private int hashedNum;
	private float dissolveExtent;

	void Awake()
	{
		if (PlayerPrefs.GetInt (Consts.VariableName.pickUpName + objectName) == 1)
			Destroy (gameObject);
		isExist = true;
		isNotified = false;
		dissolveExtent = 0f;
		hashedNum = Animator.StringToHash (objectName);
		gameController = GameObject.FindGameObjectWithTag (Tags.GameController).GetComponent<GameController> ();
		uiController = GameObject.FindGameObjectWithTag (Tags.UIController).GetComponent<UIController> ();
	}

	public override void PhotoTaken()
	{
		PlayerPrefs.GetInt (Consts.VariableName.pickUpName + objectName, 1);
		gameController.AddItem (objectName,introduction);
		if(isExist || isReusable)
		Instantiate (pickUpEffect, transform.position, Quaternion.identity);
		if (isReusable)
			return;
		isExist = false;
		DeleteNotification ();
		GetComponent<Renderer> ().material = dissolveMat;
		StartCoroutine (Dissolve());
	}

	protected IEnumerator Dissolve()
	{
		gameController.UpdateObjects ();
		DeleteNotification ();
		while (dissolveExtent <= 1f)
		{
			dissolveExtent += dissolvingSpeed * Time.deltaTime;
			GetComponent<Renderer> ().material.SetFloat ("_Amount",dissolveExtent);
			yield return null;
		}
		DeleteNotification ();
		Destroy (gameObject);
	}
}
