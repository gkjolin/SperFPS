using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {

	public GameObject pooledObject;

	[SerializeField]
	private int poolSize;
	[SerializeField]
	private bool canGrow;

	private List<GameObject> pooledGameObjects;
	private Transform trsf;

	void Awake () {
		trsf = transform;
		pooledGameObjects = new List<GameObject>();
		for(int i = 0; i < poolSize; i++)
		{
			GameObject obj = (GameObject)Instantiate(pooledObject);
			obj.SetActive(false);
			obj.transform.SetParent(trsf);

			PooledObjectDeactivation particleDeactivation = obj.GetComponent<PooledObjectDeactivation>();
			if(particleDeactivation)
			{
				particleDeactivation.parent = trsf;
			}

			pooledGameObjects.Add(obj);
		}
	}

	public GameObject GetCurrentPooledGameObject()
	{
		for(int i = 0; i < poolSize; i++)
		{
			if(!pooledGameObjects[i].activeInHierarchy)
			{
				return pooledGameObjects[i];
			}
		}

		if(canGrow)
		{
			GameObject obj = (GameObject)Instantiate(pooledObject);
			obj.transform.SetParent(trsf);
			pooledGameObjects.Add(obj);
			return obj;
		}
		return null;
	}

}
