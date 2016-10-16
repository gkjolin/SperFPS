using UnityEngine;
using System.Collections;

public class IABody : MonoBehaviour {

	[HideInInspector]
	public float orientationSpeed;
	[HideInInspector]
	public bool lookAtTarget;
	[HideInInspector]
	public bool look;
	[HideInInspector]
	public IATarget target;

	private NavMeshAgent navMeshAgent;
	private Transform trsf;

	public void SetUpIA () {
		navMeshAgent = transform.parent.GetComponent<NavMeshAgent>();
		trsf = transform;
	}

	void Update () {
		if(look)
		{
			if(lookAtTarget)
			{
				Vector3 direction = (target.position - trsf.position).normalized;
				if(direction.sqrMagnitude > 0.0f)
				{
					Orient(direction);
				}
			}
			else
			{
				Vector3 direction = navMeshAgent.velocity.normalized;
				if(direction.sqrMagnitude > 0.0f)
				{
					Orient(direction);
				}
			}
		}
	}

	void Orient (Vector3 f)
	{
		Quaternion lookRot = Quaternion.LookRotation(f, Vector3.up);
		trsf.rotation = Quaternion.Lerp(trsf.rotation, lookRot, orientationSpeed*Time.deltaTime);
	}
}
