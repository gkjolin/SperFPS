using UnityEngine;
using System.Collections;

//[SerializeField]
//public class WeaponType
//{
//	public WeaponModuleBase[] weaponModuleBases;
//	public WeaponModuleCannon[] weaponModuleCannons;
//	public WeaponModuleMagazine[] weaponModuleMagazines;
//}

[CreateAssetMenu(fileName = "WeaponGeneratorData", menuName = "ScriptableObjects/Weapons/WeaponGeneratorData", order = 6)]
public class WeaponGeneratorData : ScriptableObject {
	public WeaponModuleBase[] weaponModuleBases_0;
	public WeaponModuleCannon[] weaponModuleCannonsx1_0;
	public WeaponModuleCannon[] weaponModuleCannonsx2_0;
	public WeaponModuleCannon[] weaponModuleCannonsx3_0;
	public WeaponModuleCannon[] weaponModuleCannonsx4_0;
	public WeaponModuleMagazine[] weaponModuleMagazines_0;
}


