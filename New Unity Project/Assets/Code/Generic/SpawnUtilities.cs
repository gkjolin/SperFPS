using UnityEngine;
using System.Collections;

public class SpawnUtilities : MonoBehaviour {

	public static SpawnUtilities instance;

	void Awake () 
	{
		instance = this;
	}

	public void SetGOPositionAndDirection(GameObject go, Transform t, Vector3 position, Vector3 direction, bool setParent)
	{
		if(go)
		{
			go.transform.position = position;
			go.transform.forward = direction;
			if(setParent)
			{
				go.transform.SetParent(t);
			}
			go.SetActive(true);
		}
		else
		{
			Debug.Log("Need More go!(Probably fx or projectile)");
		}
	}

	public void SetGOPosition(GameObject go, Transform t, bool setParent)
	{
		if(go)
		{
			go.transform.position = t.position;
			if(setParent)
			{
				go.transform.SetParent(t);
			}
			go.SetActive(true);
		}
		else
		{
			Debug.Log("Need More go!(Probably audio or collectable)");
		}
	}
}
