using UnityEngine;
using System.Collections;

public class IAEye : MonoBehaviour {

	public IAEyeData data;

	[HideInInspector]
	public bool detected;
	[HideInInspector]
	public IABase iaBase;

	private Transform trsf;
	private float sqrRange;

	public void SetUpIA()
	{
		trsf = transform;
		sqrRange = data.range*data.range;
	}

	void OnEnable()
	{
		detected = false;
	}

	void Update()
	{
		if((iaBase.target.position - trsf.position).sqrMagnitude < sqrRange)
		{
			DetectTarget(iaBase.target);
		}
		else
		{
			detected = false;
		}
	}

	public void DetectTarget(IATarget target)
	{
		Vector3 dir = target.position - trsf.position;

		if(Vector3.Angle(trsf.forward, dir) < data.halfFOV)
		{
			RaycastHit hit;
			if(Physics.Raycast(trsf.position, dir, out hit, data.range, data.layer.value))
			{
				if(hit.collider.gameObject.transform == iaBase.target.parent)
				{
					detected = true;
				}
				else
				{
					detected = false;
				}
			}
			else
			{
				detected = false;
			}
		}
		else
		{
			detected = false;
		}
	}
}
