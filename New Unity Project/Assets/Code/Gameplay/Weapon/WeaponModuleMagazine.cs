﻿using UnityEngine;
using System.Collections;

public class WeaponModuleMagazine : MonoBehaviour {

	public WeaponModuleMagazineData data;

	[HideInInspector]
	public int currentMagazine;
	[HideInInspector]
	public ObjectPool projectilsPool;
	[HideInInspector]
	public ObjectPool fireFXPool;
	[HideInInspector]
	public float recoilForce;
	[HideInInspector]
	public ObjectPool reloadSoundPool;

	public void SetUpItem()
	{
		SetUpProjectile();
		SetUpSound();
		recoilForce = projectilsPool.pooledObject.GetComponent<ProjectilePlayer>().data.recoilForce;
		currentMagazine = data.magazine;
	}

	void SetUpProjectile()
	{
		switch(data.projectileType)
		{
		case GameManager.ProjectileType.Gun :
			{
				projectilsPool = ObjectPoolManager.instance.gun;
				fireFXPool = ObjectPoolManager.instance.gunFire;
				break;
			}
		case GameManager.ProjectileType.Laser :
			{
				projectilsPool = ObjectPoolManager.instance.laser;
				fireFXPool = ObjectPoolManager.instance.laserFire;
				break;
			}
		case GameManager.ProjectileType.Dart :
			{
				projectilsPool = ObjectPoolManager.instance.dart;
				fireFXPool = ObjectPoolManager.instance.dartFire;
				break;
			}
		}
	}

	void SetUpSound()
	{
		switch(data.projectileType)
		{
		case GameManager.ProjectileType.Gun :
			{
				if(data.moduleLevel == 0)
				{
					reloadSoundPool = ObjectPoolManager.instance.GunReload01_AS;
				}
				else if(data.moduleLevel == 1)
				{
					reloadSoundPool = ObjectPoolManager.instance.GunReload02_AS;
				}
				else if(data.moduleLevel == 2)
				{
					reloadSoundPool = ObjectPoolManager.instance.GunReload03_AS;
				}
				else if(data.moduleLevel == 3)
				{
					reloadSoundPool = ObjectPoolManager.instance.GunReload04_AS;
				}

				break;
			}
		case GameManager.ProjectileType.Laser :
			{
				break;
			}
		case GameManager.ProjectileType.Dart :
			{
				break;
			}
		}
	}
}
