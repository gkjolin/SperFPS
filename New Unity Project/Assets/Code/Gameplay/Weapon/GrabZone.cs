using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabZone : MonoBehaviour {

	public Transform playerHead;
	public GenericItem grabableItem;

	private List<Collider> colliders = new List<Collider>();
	private List<GenericItem> genericItems = new List<GenericItem>();
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

			GenericItem gi = c.GetComponentInParent<GenericItem>();
			if(gi && !genericItems.Contains(gi))
			{
				genericItems.Add(gi);
			}
		}
	}

	void OnTriggerExit(Collider c)
	{
		if(colliders.Contains(c))
		{
			colliders.Remove(c);

			GenericItem gi = c.GetComponentInParent<GenericItem>();
			if(gi && genericItems.Contains(gi))
			{
				gi.SetRenderers(false);
				genericItems.Remove(gi);
				if(grabableItem == gi)
				{
					grabableItem = null;
				}
			}
		}
	}

	public void RemoveFromLists(GenericItem gi)
	{
		if(colliders.Contains(gi.grabCollider))
		{
			colliders.Remove(gi.grabCollider);
		}
		if(genericItems.Contains(gi))
		{
			genericItems.Remove(gi);
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

		for(int i = 0; i < genericItems.Count; i++)
		{
			genericItems[i].SetRenderers(false);
			Vector3 d2p = genericItems[i].transform.position - playerHead.position;
			float d = Vector3.Cross(ray.direction, d2p).sqrMagnitude*d2p.sqrMagnitude;
			if(d < minDist)
			{
				minDist =  d;
				grabableItem = genericItems[i];
			}
		}

		grabableItem.SetRenderers(true);
	}
}
