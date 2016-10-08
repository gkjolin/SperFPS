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
	private bool movingBecomeTrue = false;
	private Vector3 timer;

	void Awake () {
		trsf = transform;
		initialPosition = trsf.localPosition;
		timer = Vector3.zero;
	}

	void Update () {

		if(playerMove.moving == true && movingBecomeTrue == false)
		{
			timer = positionPhase;
			movingBecomeTrue = true;
		}
		else if(playerMove.moving == false && movingBecomeTrue == true)
		{
			movingBecomeTrue = false;
		}

		if(playerMove.moving == true)
		{
			timer += Time.deltaTime*positionSpeed;
			Vector3 anm = initialPosition + new Vector3(Mathf.Sin(timer.x)*positionAmplitude.x, Mathf.Sin(timer.y)*positionAmplitude.y, Mathf.Sin(timer.z)*positionAmplitude.z);
			trsf.localPosition = Vector3.Lerp(trsf.localPosition, anm, Mathf.Clamp01(inSpeed*Time.deltaTime));
		}
		else
		{
			trsf.localPosition = Vector3.Lerp(trsf.localPosition, initialPosition, Mathf.Clamp01(outSpeed*Time.deltaTime));
		}
	}
}
