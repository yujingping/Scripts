using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class RealPicture : MonoBehaviour 
{
	public Transform[] strongPoints;
	public Transform[] weakPoints;

	[SerializeField] private float rotThreshold;
	[SerializeField] private float minDis;
	[SerializeField] private int minWeakNum;
	[SerializeField] private string pictureName;
	[SerializeField] private bool checkNormalPlane;
	[SerializeField] private bool checkPointSets;
	[SerializeField] private string invokeQuestName;
	[SerializeField] private string currentQueestName;

	private Transform m_transform;
	private Vector3 normalDirection;
	private float dComponent;

	void Awake()
	{
		m_transform = transform;
		if (checkNormalPlane)
			DetermineNormalParams ();
	}

	public bool CheckValidation(Transform trans)
	{
		bool normal = true, point = true;
		if (checkNormalPlane)
			normal = CheckNormal (trans);
		if (checkPointSets)
			point = CheckPoints (trans);
		return normal & point;
	}

	public IEnumerator TakePhoto()
	{
		DialogueManager.ShowAlert ("Photo : \"" + pictureName + "\" Has been Taken!");
		DialogueLua.SetQuestField (invokeQuestName, Consts.VariableName.state, Consts.QuestCondition.Active);
		DialogueLua.SetQuestField (currentQueestName, Consts.VariableName.state, Consts.QuestCondition.Success);
		yield return new WaitForSeconds (3f);
		Destroy (gameObject);
	}

	private void DetermineNormalParams()
	{
		normalDirection = transform.rotation * Vector3.up.normalized;
		dComponent = -Vector3.Dot (normalDirection, m_transform.position);
	}

	private bool CheckNormal(Transform trans)
	{
		Vector3 direction = (m_transform.position - trans.position).normalized;
		float approxiamtingDeg = -Vector3.Dot (direction, normalDirection);
		if (approxiamtingDeg <= rotThreshold || Vector3.Dot (normalDirection, trans.position) + dComponent < minDis)
			return false;
		return true;
	}

	private bool CheckPoints(Transform trans)
	{
		RaycastHit hit;
		foreach (Transform point in strongPoints)
		{
			if (Physics.Linecast (trans.position, point.position, out hit))
			{
				if (hit.collider.gameObject != point.gameObject)
					return false;
			}
		}
		int totWeakNum = 0;
		int weakPointNum = weakPoints.Length;
		foreach (Transform point in weakPoints)
		{
			if (Physics.Linecast (trans.position, point.position, out hit))
			{
				if (hit.collider.gameObject == point.gameObject)
					totWeakNum++;
			}
		}
		return totWeakNum >= minWeakNum;
	}

}
