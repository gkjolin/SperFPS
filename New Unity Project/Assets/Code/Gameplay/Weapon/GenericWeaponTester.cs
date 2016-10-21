using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericWeaponTester : MonoBehaviour {

	public WeaponModuleBase[] weaponModuleBases;
	public WeaponModuleMagazine[] weaponModuleMagazines;
	public WeaponModuleCannon[] weaponModuleCannons;

	public float damages;

	void Start () {
		CalculateDpsCurve();
	}

	void CalculateDpsCurve()
	{
		List<float> res = new List<float>();
		for(int i = 0; i < weaponModuleBases.Length; i++)
		{
			for(int k = 0; k < weaponModuleCannons.Length; k++)
			{
				float dps = CalculateDps(weaponModuleBases[i], weaponModuleMagazines[0], weaponModuleCannons[k]);
				res.Add(dps);
				Debug.Log("B"+ weaponModuleBases[i].data.moduleLevel + "M" + weaponModuleMagazines[0].data.moduleLevel + "C" + weaponModuleCannons[k].data.moduleLevel + " x" + weaponModuleCannons[k].cannons.Length + " (" + dps + ")");
			}
		}
	}

	float CalculateDps(WeaponModuleBase wmb, WeaponModuleMagazine wmm, WeaponModuleCannon wmc)
	{
		float rate1 = wmb.data.rafaleRate*wmm.data.fireRateLimiter*(1.0f + (wmc.cannons.Length-1.0f)*wmc.data.multiCannonFireRateLimiter);
		float rate2 = wmb.data.fireRate*wmm.data.fireRateLimiter*(1.0f + (wmc.cannons.Length-1.0f)*wmc.data.multiCannonFireRateLimiter);

		float rate = (rate1*(wmb.data.rafaleCount-1.0f) + rate2)/wmb.data.rafaleCount;

		return (wmc.cannons.Length*damages)/rate;
	}
}
