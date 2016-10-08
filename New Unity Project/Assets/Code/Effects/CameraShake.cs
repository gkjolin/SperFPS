using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public float amplitude;
	public float frequency;
	public float duration;
	public AnimationCurve stepCurve;

	private Transform trsf;
	private Quaternion initialRotation;
	private float currentAmplitude;
	private bool shaking;

	void Awake () {
		trsf = transform;
		initialRotation = trsf.localRotation;
		currentAmplitude = 0.0f;
	}

	void Update () {
		
		if(currentAmplitude > 0.0f && shaking == false)
		{
			StartCoroutine(Shake());
		}

		if(currentAmplitude > 0.0f)
		{
			currentAmplitude -= Time.deltaTime/duration;
		}
	}

	IEnumerator Shake()
	{
		shaking = true;

		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		Quaternion oldRotation = trsf.localRotation;
		Vector3 rot = initialRotation.eulerAngles;
		rot.x += Random.Range(-currentAmplitude,currentAmplitude);
		rot.y += Random.Range(-currentAmplitude,currentAmplitude);
		rot.z += Random.Range(-currentAmplitude,currentAmplitude);
		Quaternion nextRotation = Quaternion.Euler(rot);

		float value = 0.0f;
		while( value < frequency)
		{
			value += Time.deltaTime;
			trsf.localRotation = Quaternion.Lerp(oldRotation, nextRotation, stepCurve.Evaluate(value/frequency));
			yield return wait;
		}

		trsf.localRotation = nextRotation;

		shaking = false;
	}

	public void SetAmplitude()
	{
		currentAmplitude = amplitude;
	}
}
