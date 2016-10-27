using UnityEngine;
using System.Collections;

public class HandMovement : MonoBehaviour {

	public PlayerMove playerMove;

	public Vector3 positionAmplitude;
	public Vector3 positionSpeed;
	public Vector3 positionPhase;

	public float inSpeed;
	public float outSpeed;

	public bool stepSound;
	public float stepSoundFrequency;

	[HideInInspector]
	public bool playStepSound = false;

	private Transform trsf;
	private Vector3 initialPosition;
	private bool movingBecomeTrue = false;
	private Vector3 timer;
	private float timerStep;
	private bool canPlayStepSound = false;

	void Awake () {
		trsf = transform;
		initialPosition = trsf.localPosition;
		timer = Vector3.zero;
	}

	void Update () {
		if(playerMove.moving == true && movingBecomeTrue == false)
		{
			timer = positionPhase;//new Vector3(positionPhase.x*positionSpeed.x, positionPhase.y*positionSpeed.y, positionPhase.z*positionSpeed.z);
			timerStep = positionPhase.y;
			movingBecomeTrue = true;
		}
		else if(playerMove.moving == false && movingBecomeTrue == true)
		{
			movingBecomeTrue = false;
		}
		if(playerMove.moving == true)
		{
			timer += Time.deltaTime*positionSpeed*playerMove.actualSpeed;
			Vector3 sinus = new Vector3(Mathf.Sin(timer.x), Mathf.Sin(timer.y), Mathf.Sin(timer.z));

			if(stepSound)
			{
				timerStep += Time.deltaTime*playerMove.actualSpeed*stepSoundFrequency;
				float sinusStep = Mathf.Sin(timerStep);
				if(sinusStep > 0.9f && canPlayStepSound == true)
				{
					playStepSound = true;
					canPlayStepSound = false;
				}
				if(sinusStep < 0.0f && canPlayStepSound == false)
				{
					canPlayStepSound = true;
				}
			}

			Vector3 anm = initialPosition + new Vector3(sinus.x*positionAmplitude.x, sinus.y*positionAmplitude.y, sinus.z*positionAmplitude.z);
			trsf.localPosition = Vector3.Lerp(trsf.localPosition, anm, Mathf.Clamp01(inSpeed*Time.deltaTime));
		}
		else
		{
			trsf.localPosition = Vector3.Lerp(trsf.localPosition, initialPosition, Mathf.Clamp01(outSpeed*Time.deltaTime));
		}
	}
}
