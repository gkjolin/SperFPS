﻿using UnityEngine;
using System.Collections;

public class GenericInteractable : MonoBehaviour {

	[HideInInspector]
	public HighLightSystem highLightSystem;
	[HideInInspector]
	public Collider grabCollider;

	void Awake () {
		highLightSystem = GetComponent<HighLightSystem>();
		highLightSystem.SetUp();
		highLightSystem.material = GameManager.instance.highLightMaterial_01;
		grabCollider = GetComponent<Collider>();
	}
}