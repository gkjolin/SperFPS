using UnityEngine;
using System.Collections;

public class SpawnUtilities : MonoBehaviour {

	public static SpawnUtilities instance;

	void Awake () 
	{
		instance = this;
	}

	public void SetFx(GameObject go, Transform t, Vector3 position, Vector3 direction, bool setParent)
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
			Debug.Log("Need More fx!");
		}
	}

	public void SetAudio(GameObject go, Transform t, bool setParent)
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
			Debug.Log("Need More audio!");
		}
	}
}
