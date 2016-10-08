using UnityEngine;
using System.Collections;

public class PooledObjectDeactivation : MonoBehaviour {

	public float timer;

	protected Transform trsf;
	[HideInInspector]
	public Transform parent;

	protected virtual void Awake () {
		trsf = transform;
	}

	protected virtual void OnEnable () {
		if(timer > 0.0f)
		{
			Invoke("Disable", timer);
		}
	}

	public void Disable()
	{
		gameObject.SetActive(false);

		if(parent)
		{
			trsf.SetParent(parent); 
			trsf.localScale = Vector3.one;
		}
	}
		
}
