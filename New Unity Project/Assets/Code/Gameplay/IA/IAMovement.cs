using UnityEngine;
using System.Collections;

public class IAMovement : MonoBehaviour {

	[HideInInspector]
	public NavMeshAgent navMeshAgent;

	void Awake () {
		navMeshAgent = GetComponent<NavMeshAgent>();
	}
		
	public void GotoDestination(Vector3 position)
	{
		navMeshAgent.SetDestination(position);
	}

	public void Strafe(Vector3 offset)
	{
		navMeshAgent.Move(offset);
	}

	public void Stop()
	{
		navMeshAgent.Stop();
	}

	public void Resume()
	{
		navMeshAgent.Resume();
	}

	public bool isAtDestination()
	{
		if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && navMeshAgent.velocity.sqrMagnitude == 0.0f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
