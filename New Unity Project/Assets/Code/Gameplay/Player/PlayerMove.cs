using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public float speed;
	public float jumpForce;
	public float fallForce;
	public float airControl;
	public float limitAirSpeed;
	public Transform head;
	public float playerEquipmentWeightInfluence;

	[HideInInspector]
	public bool moving;
	[HideInInspector]
	public float playerEquipmentWeight;
	[HideInInspector]
	public bool stuned;
	[HideInInspector]
	public Rigidbody rgdBody;
	[HideInInspector]
	public bool jumping;
	[HideInInspector]
	public CheckGround checkGround;

	private Vector3 moveDirection = new Vector3(0.0f,0.0f,0.0f);
	private float jump;
	private float equipmentWeight;

	void Awake () {
		rgdBody = GetComponent<Rigidbody>();
		checkGround = GetComponent<CheckGround>();
		jumping = false;
		stuned = false;
		SetEquipmentWeight();
	}

	void Update () {

		if(Input.GetAxis("Vertical") != 0.0f && checkGround.grounded == true)
		{
			moving = true;
		}
		else if(Input.GetAxis("Horizontal") != 0.0f && checkGround.grounded == true)
		{
			moving = true;
		}
		else
		{
			moving = false;
		}

		if(moving == true)
		{
			moveDirection = head.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0.0f, -Input.GetAxis("Vertical")));
		}
		else
		{
			moveDirection = head.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0.0f, -Input.GetAxis("Vertical")));
		}

		if(Input.GetAxis("Jump") > 0.0f && checkGround.grounded == true)
		{
			jump = jumpForce/equipmentWeight;
		}
		else
		{
			jump = 0.0f;
		}
	}

	IEnumerator WaitForJump()
	{
		jumping = true;
		yield return new WaitForSeconds(0.1f);
		jumping = false;
	}

	void FixedUpdate()
	{
		if(checkGround.grounded)
		{
			rgdBody.velocity = new Vector3(moveDirection.x*speed, rgdBody.velocity.y, moveDirection.z*(speed/equipmentWeight));
		}
		else
		{
			rgdBody.AddForce(new Vector3(moveDirection.x, 0.0f, moveDirection.z)*(airControl/equipmentWeight), ForceMode.Acceleration);
			rgdBody.AddForce(new Vector3(0.0f, -fallForce, 0.0f),  ForceMode.Force);

			Vector3 newVelocity = rgdBody.velocity;
			newVelocity.x = Mathf.Clamp(newVelocity.x, -limitAirSpeed, limitAirSpeed);
			newVelocity.z = Mathf.Clamp(newVelocity.z, -limitAirSpeed, limitAirSpeed);

			rgdBody.velocity = newVelocity;
		}

		if(jump > 0.0 && jumping == false)
		{
			StartCoroutine(WaitForJump());
			rgdBody.AddForce(new Vector3(0.0f, jump, 0.0f),  ForceMode.Impulse);
		}
	}

	public void SetEquipmentWeight()
	{
		equipmentWeight = (1.0f + playerEquipmentWeight*playerEquipmentWeightInfluence);
	}
}
