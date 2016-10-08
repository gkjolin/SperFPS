using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {

	public Transform body;
	public float sensitivity;
	public bool invert;
	public float minY;
	public float maxY;

	private Transform trsf;
	private float rotationX;
	private float rotationY;

	void Awake () {
		trsf = transform;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		rotationX = 0.0f;
		rotationY = 0.0f;
	}

	void Update () {

		Vector2 mInput;

		if(invert)
		{
			mInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))*Time.deltaTime*sensitivity;
		}
		else
		{
			mInput = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"))*Time.deltaTime*sensitivity;
		}

		rotationX += mInput.x;
		rotationY += mInput.y;
		rotationY = Mathf.Clamp(rotationY, minY, maxY);

		trsf.localRotation = Quaternion.Euler(new Vector3(rotationY, 0.0f, 0.0f));
		body.localRotation = Quaternion.Euler(new Vector3(0.0f, rotationX, 0.0f));
	}
}
