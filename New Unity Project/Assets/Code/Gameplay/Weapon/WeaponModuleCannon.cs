using UnityEngine;
using System.Collections;

public class WeaponModuleCannon : MonoBehaviour {

	public WeaponModuleCannonData data;
	public Transform[] cannons;

	[HideInInspector]
	public ObjectPool shootSoundPool;

	void Awake()
	{
		SetUpSound();
	}

	void SetUpSound()
	{
		switch(data.projectileType)
		{
		case GameManager.ProjectileType.Gun :
			{
				if(cannons.Length == 1)
				{
					shootSoundPool = ObjectPoolManager.instance.GunShotx1AU;
				}
				else if(cannons.Length == 2)
				{
					shootSoundPool = ObjectPoolManager.instance.GunShotx2AU;
				}
				else if(cannons.Length == 3)
				{
					shootSoundPool = ObjectPoolManager.instance.GunShotx3AU;
				}
				else if(cannons.Length == 4)
				{
					shootSoundPool = ObjectPoolManager.instance.GunShotx4AU;
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
