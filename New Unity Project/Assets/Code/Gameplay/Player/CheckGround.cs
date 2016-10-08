using UnityEngine;
using System.Collections;

public class CheckGround : MonoBehaviour {

	public float offsetY;
	public float sphereRadius;
	public string[] layers; 
	[HideInInspector]
	public bool grounded;

	private Transform trsf;
	private LayerMask layer;

	void Awake()
	{
		trsf = transform;
		layer = LayerMask.GetMask(layers);
	}

	void FixedUpdate () {
		if(Physics.CheckSphere(trsf.position + new Vector3(0.0f, offsetY, 0.0f), sphereRadius, layer.value))
		{
			grounded = true;
		}
		else
		{
			grounded = false;
		}
	}

//	public bool CheckWall(Vector3 dir)
//	{
//		RaycastHit hit;
//		if(Physics.Raycast(trsf.position + new Vector3(0.0f, checkWallRayOffsetY, 0.0f), dir, out hit, checkWallRayDistance, layer.value))
//		{
//			Debug.Log(Mathf.Abs(Vector3.Dot(hit.normal, Vector3.up)));
//			if(Mathf.Abs(Vector3.Dot(hit.normal, Vector3.up)) < checkWallRayThreshold)
//			{
//				return true;
//			}
//		}
//
//		return false;
//	}
}