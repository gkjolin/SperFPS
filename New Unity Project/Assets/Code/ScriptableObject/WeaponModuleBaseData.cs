using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "WeaponModuleBaseData", menuName = "ScriptableObjects/Weapons/WeaponModuleBaseData", order = 3)]
public class WeaponModuleBaseData : ScriptableObject {

	public int moduleLevel;

	public GameManager.ProjectileType projectileType;

	public float fireRate;
	public float rafaleRate;
	public int rafaleCount;
	public Vector3 weaponRecoilPosition;
	public float weaponRecoilRotation;
	public Vector3 weaponRecoilRotationAxis;
	public AnimationCurve weaponFireCurve;
	public Vector3 weaponReloadPosition;
	public float weaponReloadRotation;
	public Vector3 weaponReloadRotationAxis;
	public AnimationCurve weaponReloadCurve;
	public float mass;
}
