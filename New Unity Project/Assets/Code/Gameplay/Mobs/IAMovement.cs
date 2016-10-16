using UnityEngine;
using System.Collections;

public class IAMovement : MonoBehaviour {

	public IAMovementData data;

	[HideInInspector]
	public NavMeshAgent navMeshAgent;

	public void SetUpIA () {
		navMeshAgent.speed = data.speed;
		navMeshAgent.acceleration = data.acceleration;
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
