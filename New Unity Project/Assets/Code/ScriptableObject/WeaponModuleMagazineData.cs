using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "WeaponModuleMagazineData", menuName = "ScriptableObjects/Weapons/WeaponModuleMagazineData", order = 4)]
public class WeaponModuleMagazineData : ScriptableObject {

	public int moduleLevel;

	public GameManager.ProjectileType projectileType;

	public float fireRateLimiter;
	public int magazine;
	public float reloadTime;
	public float reloadSoundDelay;
	public float shakeAmplitude;
	public float shakeFrequency;
	public float shakeDuration;

	public float mass;

}
