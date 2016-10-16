using UnityEngine;
using System.Collections;

//[SerializeField]
//public class WeaponType
//{
//	public WeaponModuleBase[] weaponModuleBases;
//	public WeaponModuleCannon[] weaponModuleCannons;
//	public WeaponModuleMagazine[] weaponModuleMagazines;
//}

[CreateAssetMenu(fileName = "MobGeneratorData", menuName = "ScriptableObjects/Mobs/MobGeneratorData", order = 5)]
public class MobGeneratorData : ScriptableObject {
	public IABase[] IABase_0;
	public IAWeapon[] IAWeapon_0;
	public IAEye[] IAEye_0;
	public IAMovement[] IAMovement_0;
}


