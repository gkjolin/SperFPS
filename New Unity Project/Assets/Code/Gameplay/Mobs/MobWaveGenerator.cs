using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobWaveGenerator : MonoBehaviour {

	public static MobWaveGenerator instance;
	public List<GameObject> mobList = new List<GameObject>();

	void Awake () {
		instance = this;
	}
}
