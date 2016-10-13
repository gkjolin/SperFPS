using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "WeaponModuleCannonData", menuName = "ScriptableObjects/Weapons/WeaponModuleCannonData", order = 5)]
public class WeaponModuleCannonData : ScriptableObject {
	
	public int moduleSize;
	public int moduleLevel;

	public GameManager.ProjectileType projectileType;

	public float precision;
	public float weaponNoise;
	public float multiCannonFireRateLimiter;

	public float mass;


}
