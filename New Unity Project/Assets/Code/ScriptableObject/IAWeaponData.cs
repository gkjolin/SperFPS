using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "IAWeaponData", menuName = "ScriptableObjects/Mobs/IAWeaponData", order = 3)]
public class IAWeaponData : ScriptableObject {

	public GameManager.HostileProjectileType hostileProjectileType;

	public Vector2 attackFrequency;
	public int minShot;
	public int maxShot;
	public float fireRate;
	public float precision;
	public bool avoidFriendlyFire;
	public LayerMask friendlyLayer;

}
