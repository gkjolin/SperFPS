using UnityEngine;
using System.Collections;

public class ProjectileActivation : MonoBehaviour {

	[SerializeField]
	protected float duration;

	void OnEnable () {
		Invoke("SetInactive", duration);
	}
	

	void SetInactive () {
		gameObject.SetActive(false);
	}

	void OnDisable () {
		CancelInvoke();
	}
}
