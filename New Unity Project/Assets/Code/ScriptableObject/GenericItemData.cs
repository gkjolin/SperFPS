using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "GenericItemData", menuName = "ScriptableObjects/Weapons/GenericItemData", order = 2)]
public class GenericItemData : ScriptableObject {
	public float grabTime;
	public AnimationCurve weaponGrabCurve;
	public Vector3 weaponGrabPosition;
	public float weaponGrabRotation;
	public Vector3 weaponGrabRotationAxis;
}
