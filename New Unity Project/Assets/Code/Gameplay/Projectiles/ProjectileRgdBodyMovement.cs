using UnityEngine;
using System.Collections;

public class ProjectileRgdBodyMovement : MonoBehaviour {

	public Vector3 force;
	private Rigidbody rgdBody;
	private bool addForce;

	void Awake()
	{
		rgdBody = GetComponent<Rigidbody>();
		addForce = false;
	}

	void OnEnable () {
		addForce = true;
	}
		
	void FixedUpdate()
	{
		if(addForce == true)
		{
			addForce = false;
			rgdBody.velocity = Vector3.zero;
			rgdBody.AddForce(force, ForceMode.Impulse);
		}
	}
}
