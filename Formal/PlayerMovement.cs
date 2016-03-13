using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	public float turnSmoothing;
	public float speedDampTime;

	private GameObject playerGameObject;
	private Animator anim;
	private DoneHashIDs hash;

	[SerializeField] private float runSpeed;
	[SerializeField] private float walkSpeed;

	void Awake()
	{
		playerGameObject = transform.FindChild (Tags.Player).gameObject;
		anim = playerGameObject.GetComponent<Animator> ();
		hash = GameObject.FindGameObjectWithTag (Tags.GameController).GetComponent<DoneHashIDs> ();
	}

	public void MovementManage(float horizontal,float vertical,bool sneak)
	{
		//anim.SetBool(hash.sneakingBool,sneak);
		if(horizontal != 0 || vertical != 0)
		{
			Rotating(horizontal,vertical);
			anim.SetFloat(hash.speedFloat,sneak ? walkSpeed : runSpeed, speedDampTime, Time.deltaTime);
		}
		else 
			anim.SetFloat(hash.speedFloat,0f);
	}

	void Rotating(float horizontal,float vertical)
	{
		float yAxisRotation = transform.rotation.eulerAngles.y;
		Vector3 targetDirection = new Vector3 (horizontal,0f,vertical);
		Quaternion targetRotation = Quaternion.LookRotation (targetDirection,Vector3.up);
		targetRotation *= Quaternion.Euler (0f,yAxisRotation,0f);
		Quaternion newRotation = Quaternion.Slerp(playerGameObject.GetComponent<Rigidbody>().rotation,targetRotation,turnSmoothing * Time.deltaTime);
		playerGameObject.GetComponent<Rigidbody> ().MoveRotation (newRotation);
	}
}
