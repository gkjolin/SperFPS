using UnityEngine;
using System.Collections;

public class HandMovement : MonoBehaviour {

	public PlayerMove playerMove;

	public Vector3 positionAmplitude;
	public Vector3 positionSpeed;
	public Vector3 positionPhase;

	public float inSpeed;
	public float outSpeed;

	private Transform trsf;
	private Vector3 initialPosition;

	void Awake () {
		trsf = transform;
		initialPosition = trsf.localPosition;
	}

	void Update () {

		Vector3 v = positionSpeed*Time.time + positionPhase;
		Vector3 anm = initialPosition + new Vector3(Mathf.Sin(v.x)*positionAmplitude.x, Mathf.Sin(v.y)*positionAmplitude.y, Mathf.Sin(v.z)*positionAmplitude.z);

		if(playerMove.moving == true)
		{
			trsf.localPosition = Vector3.Lerp(trsf.localPosition, anm, Mathf.Clamp01(inSpeed*Time.deltaTime));
		}
		else
		{
			trsf.localPosition = Vector3.Lerp(trsf.localPosition, initialPosition, Mathf.Clamp01(outSpeed*Time.deltaTime));
		}
	}
}
