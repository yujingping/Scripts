using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NPCNotifier : MonoBehaviour 
{
	public ConversationController conversationController;
	public UnityEngine.UI.Button button;

	[SerializeField] private float thresoldAngle;
	[SerializeField] private float detectionAngle;
	[SerializeField] private float detectionDistance;

	private GameController gameController;
	private List<GameObject> npcs = new List<GameObject> ();
	private Camera _camera;
	private Transform m_camTrans;
	private Transform m_playerTrans;

	GameObject npc_selected = null;

	void Awake()
	{
		_camera = Camera.main;
		m_camTrans = _camera.transform;
		m_playerTrans = transform.FindChild (Tags.Player).transform;
		gameController = GameObject.FindGameObjectWithTag (Tags.GameController).GetComponent<GameController> ();
	}

	void Start()
	{
		StartCoroutine (GetNPC ());
		StartCoroutine (NotifyNPC ());
	}

	IEnumerator GetNPC()
	{
		while (true)
		{
			npcs = gameController.NPCs;
			yield return new WaitForSeconds (1);
		}
	}

	IEnumerator NotifyNPC()
	{
		while (true)
		{
			RaycastHit hit;
			float mininumAngle = detectionAngle;
			GameObject tempNPC = null;
			foreach(GameObject npc in npcs)
			{
				if (Vector3.Distance (m_camTrans.position, npc.transform.position) > detectionDistance)
					continue;
				if (Physics.Linecast (m_camTrans.position, npc.transform.position, out hit))
				{
					if (hit.collider.gameObject != npc)
						continue;
					Vector3 direction = (npc.transform.position - m_camTrans.position).normalized;
					float tempAngle = Vector3.Angle (direction,m_camTrans.forward);
					if (tempAngle < mininumAngle)
					{
						mininumAngle = tempAngle;
						tempNPC = npc;
					}
				}
			}
			if (tempNPC == null)
			{
				npc_selected = null;
				button.GetComponent<Image> ().enabled = false;
				button.enabled = false;
			}
			else
			{
				npc_selected = tempNPC;
				button.GetComponent<Image> ().enabled = true;
				button.enabled = true;
				Vector2 screenPos = _camera.WorldToScreenPoint (npc_selected.transform.position);
				button.GetComponent<RectTransform> ().position = new Vector3 (screenPos.x, screenPos.y, 0f);
			}
			yield return null;
		}
	}

	public void SetNotifyState(bool state)
	{
		StopCoroutine (NotifyNPC ());
		if (state)
		{
			button.gameObject.SetActive (true);
			StartCoroutine (NotifyNPC ());
		} else
			button.gameObject.SetActive (false);
	}

	public void StartConversation()
	{
		if (npc_selected == null)
			return;
		conversationController.StartConversation (npc_selected);
		//transform.FindChild ("Player").LookAt(npc_selected.transform.position);
		StartCoroutine (FacingNPC());
	}

	IEnumerator FacingNPC()
	{
		while (true)
		{
			if (npc_selected == null)
				yield return null;
			Vector3 direction = (npc_selected.transform.position - m_playerTrans.position).normalized;
			float angle = Vector3.Angle (m_playerTrans.forward, direction);
			if (angle < thresoldAngle)
				break;
			Quaternion targetRot = Quaternion.LookRotation (direction);
			m_playerTrans.rotation = Quaternion.Slerp (m_playerTrans.rotation, targetRot, Time.deltaTime * 2f);
			yield return null;
		}
	}
}
