using UnityEngine;
using System.Collections;

public class FXUtilities : MonoBehaviour {

	public static FXUtilities instance;

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
			Debug.Log("Neen More Chicken!");
		}
	}
}
