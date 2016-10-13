using UnityEngine;
using System.Collections;

public class WeaponGenerator : MonoBehaviour {

	public int[] weaponTypes;
	public int weaponLVLMin;
	public int weaponLVLMax;
	public Vector4 cannonNumbersProbability;

	public WeaponGeneratorData data;

	private int weaponType;
	private int weaponLVL;

	private WeaponModuleBase weaponModuleBase;
	private WeaponModuleCannon weaponModuleCannon;
	private WeaponModuleMagazine weaponModuleMagazine;

	private Transform trsf;

	void Awake()
	{
		trsf = transform;
		GenerateWeapon();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.N))
		{
			GenerateWeapon();
		}
	}

	public void GenerateWeapon()
	{
		weaponType = weaponTypes[Random.Range(0, weaponTypes.Length)];
		weaponLVL = Random.Range(weaponLVLMin, weaponLVLMax+1);

		GetModules(weaponType, weaponLVL);

		GameObject baseObject = Instantiate(weaponModuleBase.gameObject, trsf.position, trsf.rotation) as GameObject;
		Transform bTransform = baseObject.transform;
		Instantiate(weaponModuleCannon.gameObject, bTransform.position, bTransform.rotation, bTransform);
		Instantiate(weaponModuleMagazine.gameObject, bTransform.position, bTransform.rotation, bTransform);

		GenericWeapon gw = baseObject.GetComponent<GenericWeapon>();
		gw.SetUpItem();

		float dps = gw.dps;

		baseObject.name = weaponModuleMagazine.data.projectileType.ToString() + " LVL " + weaponLVL 
						+ "(B" + weaponModuleBase.data.moduleLevel + "C" + weaponModuleCannon.data.moduleLevel + "M" + weaponModuleMagazine.data.moduleLevel
						+ ") X" + weaponModuleCannon.cannons.Length + " (" + dps.ToString("F1") + "dps)";
	}

	void GetModules(int type, int level)
	{
		WeaponModuleBase[] wmb;
		WeaponModuleMagazine[] wmm;

		WeaponModuleCannon[] wmcx1;
		WeaponModuleCannon[] wmcx2;
		WeaponModuleCannon[] wmcx3;
		WeaponModuleCannon[] wmcx4;

		if(type == 0)
		{
			wmb = data.weaponModuleBases_0;
			wmm = data.weaponModuleMagazines_0;

			wmcx1 = data.weaponModuleCannonsx1_0;
			wmcx2 = data.weaponModuleCannonsx2_0;
			wmcx3 = data.weaponModuleCannonsx3_0;
			wmcx4 = data.weaponModuleCannonsx4_0;
		}
		else //Temporary
		{
			wmb = data.weaponModuleBases_0;
			wmm = data.weaponModuleMagazines_0;

			wmcx1 = data.weaponModuleCannonsx1_0;
			wmcx2 = data.weaponModuleCannonsx2_0;
			wmcx3 = data.weaponModuleCannonsx3_0;
			wmcx4 = data.weaponModuleCannonsx4_0;
		}

		int[] r = {Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4)};

		int loopcount = 0;

		while(r[0] + r[1] + r[2] != level)
		{
			int i = loopcount%3;
			if(r[0] + r[1] + r[2] > level)
			{
				if(r[i]-1 >= 0)
				{
					r[i] -= 1;
				}
			}
			else
			{
				if(r[i]+1 <= 3)
				{
					r[i] += 1;
				}
			}
			loopcount += 1;
		}

		GetBase(wmb, r[0]);
		GetCannon(wmcx1, wmcx2, wmcx3, wmcx4, r[1]);
		GetMagazine(wmm, r[2]);
	}

	void GetBase(WeaponModuleBase[] wmb, int level)
	{
		for(int i = 0; i < wmb.Length; i ++)
		{
			if(wmb[i].data.moduleLevel == level)
			{
				weaponModuleBase = wmb[i];
			}
		}
	}
		
	void GetCannon(WeaponModuleCannon[] wmcx1, WeaponModuleCannon[] wmcx2, WeaponModuleCannon[] wmcx3, WeaponModuleCannon[] wmcx4, int level)
	{
		for(int i = 0; i < wmcx1.Length; i ++)
		{
			if(wmcx1[i].data.moduleLevel == level)
			{
				float r = Random.Range(0.0f, 1.0f);

				if(r < cannonNumbersProbability.x)
				{
					weaponModuleCannon = wmcx1[i];
				}
				else if(r < cannonNumbersProbability.x+cannonNumbersProbability.y)
				{
					weaponModuleCannon = wmcx2[i];
				}
				else if(r < cannonNumbersProbability.x+cannonNumbersProbability.y+cannonNumbersProbability.z)
				{
					weaponModuleCannon = wmcx3[i];
				}
				else
				{
					weaponModuleCannon = wmcx4[i];
				}
			}
		}
	}

	void GetMagazine(WeaponModuleMagazine[] wmm, int level)
	{
		for(int i = 0; i < wmm.Length; i ++)
		{
			if(wmm[i].data.moduleLevel == level)
			{
				weaponModuleMagazine = wmm[i];
			}
		}
	}
}
