using UnityEngine;
using System.Collections;

public class ButtonSwitch : MonoBehaviour {

	public bool startStat;
	public bool autoSwitchBack;
	public bool OneTime;
	public bool continuous;

	public Material onMaterial;
	public Vector3 movementAxis;
	public float speedIn;
	public AnimationCurve animationCurveIn;
	public float speedOut;
	public AnimationCurve animationCurveOut;
	public float waitBetweenInAndOut;

	[HideInInspector]
	public bool onOff;
	[HideInInspector]
	public bool interacting;

	private Transform trsf;
	private Vector3 startPos;
	private Vector3 endPos;
	private Material sharedMaterial;
	private bool oneTimeDone;
	private Renderer rdr;

	private Coroutine lastCoroutine = null;
	private HighLightSystem highLightSystem;

	void Awake () 
	{
		trsf = transform;
		startPos = trsf.position;
		endPos = trsf.position + trsf.right*movementAxis.x + trsf.up*movementAxis.y + trsf.forward*movementAxis.z;
		onOff = startStat;
		oneTimeDone = false;
		rdr = GetComponent<Renderer>();
		sharedMaterial = rdr.sharedMaterial;
		SetMaterial();
		highLightSystem = GetComponent<HighLightSystem>();
		interacting = false;
	}

	public void Switch()
	{
		if(lastCoroutine == null && oneTimeDone == false)
		{
			if(onOff == false)
			{
				lastCoroutine = StartCoroutine(SwitchIn(true));
			}
			else
			{
				lastCoroutine = StartCoroutine(SwitchIn(false));
			}
		}
	}

	IEnumerator SwitchIn(bool b)
	{
		highLightSystem.enabled = false;
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		float value = 0.0f;
		while(value < 1.0f)
		{
			value += Time.deltaTime*speedIn;
			float ev = animationCurveIn.Evaluate(value);
			trsf.position = Vector3.Lerp(startPos, endPos, ev);
			yield return wait;
		}

		onOff = b;
		Debug.Log(onOff);
		SetMaterial();

		if(continuous)
		{
			while(interacting)
			{
				yield return wait;
			}
		}
		else
		{
			yield return new WaitForSeconds(waitBetweenInAndOut);
		}

		if(OneTime == false)
		{
			value = 0.0f;
			while(value < 1.0f)
			{
				value += Time.deltaTime*speedOut;
				float ev = animationCurveOut.Evaluate(value);
				trsf.position = Vector3.Lerp(endPos, startPos, ev);
				yield return wait;
			}
			highLightSystem.enabled = true;
		}
		else
		{
			oneTimeDone = true;
		}

		if(autoSwitchBack == true)
		{
			onOff = !b;
			Debug.Log(onOff);
			SetMaterial();
		}
			
		lastCoroutine = null;
	}

	void SetMaterial()
	{
		if(onMaterial)
		{
			if(onOff == true)
			{
				rdr.material = onMaterial;
			}
			else
			{
				rdr.material = sharedMaterial;
			}
		}
	}
}
