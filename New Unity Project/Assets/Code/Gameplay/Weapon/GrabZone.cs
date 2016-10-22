using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabZone : MonoBehaviour {

	public Transform playerHead;
	public GenericInteractable grabableItem;

	private List<Collider> colliders = new List<Collider>();
	private List<GenericInteractable> genericInteractables = new List<GenericInteractable>();
	private float radius;

	void Awake()
	{
		radius = GetComponent<SphereCollider>().radius;
	}

	void OnTriggerEnter(Collider c)
	{
		if(!colliders.Contains(c))
		{
			colliders.Add(c);

			GenericInteractable gi = c.GetComponentInParent<GenericInteractable>();
			if(gi && !genericInteractables.Contains(gi))
			{
				genericInteractables.Add(gi);
			}
		}
	}

	void OnTriggerExit(Collider c)
	{
		if(colliders.Contains(c))
		{
			colliders.Remove(c);

			GenericInteractable gi = c.GetComponentInParent<GenericInteractable>();

			if(gi && genericInteractables.Contains(gi))
			{
				gi.highLightSystem.hightLight = false;
				genericInteractables.Remove(gi);
				if(grabableItem == gi)
				{
					grabableItem = null;
				}
			}
		}
	}

	public void RemoveFromLists(GenericInteractable gi)
	{
		if(colliders.Contains(gi.grabCollider))
		{
			colliders.Remove(gi.grabCollider);
		}
		if(genericInteractables.Contains(gi))
		{
			genericInteractables.Remove(gi);
		}
		if(grabableItem == gi)
		{
			grabableItem = null;
		}
	}

	void OnTriggerStay()
	{
		TestDistanceToPlayer();
	}

	void TestDistanceToPlayer()
	{
		float minDist = float.PositiveInfinity;
		Ray ray = new Ray(playerHead.position, playerHead.forward*radius*2.0f);

		for(int i = 0; i < genericInteractables.Count; i++)
		{
			genericInteractables[i].highLightSystem.hightLight = false;
			Vector3 d2p = genericInteractables[i].transform.position - playerHead.position;
			float d = Vector3.Cross(ray.direction, d2p).sqrMagnitude*d2p.sqrMagnitude;
			if(d < minDist)
			{
				minDist =  d;
				grabableItem = genericInteractables[i];
			}
		}

		if(grabableItem)
		{
			grabableItem.highLightSystem.hightLight = true;
		}
	}
}
