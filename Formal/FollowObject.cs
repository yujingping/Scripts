using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour 
{
	public Transform target;

	public float offsetX;
	public float offsetY;
	public float offsetZ;

	void Awake()
	{
		transform.position = new Vector3 (target.transform.position.x + offsetX,
			target.transform.position.y + offsetY,
			target.transform.position.z + offsetZ);
	}
}
