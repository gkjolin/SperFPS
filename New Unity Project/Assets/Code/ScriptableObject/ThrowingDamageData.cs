using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ThrowingDamageData", menuName = "ScriptableObjects/Weapons/ThrowingDamageData", order = 1)]
public class ThrowingDamageData : ScriptableObject {
	public int throwingDamage;
	public float throwingStun;
	public float impactForce;
	public float squaredVelocityThreshold;
	public LayerMask damageLayer;
}
