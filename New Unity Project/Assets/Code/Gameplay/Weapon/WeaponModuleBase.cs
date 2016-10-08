using UnityEngine;
using System.Collections;

public class WeaponModuleBase : MonoBehaviour {

	public WeaponModuleBaseData data;

	[HideInInspector]
	public int currentRafaleCount;
	[HideInInspector]
	public ObjectPool grabSoundPool;

	void Awake()
	{
		currentRafaleCount = data.rafaleCount;
		SetUpSound();
	}

	void SetUpSound()
	{
		switch(data.projectileType)
		{
		case GameManager.ProjectileType.Gun :
			{
				if(data.moduleLevel == 0)
				{
					grabSoundPool = ObjectPoolManager.instance.GunGrab01AU;
				}
				else if(data.moduleLevel == 1)
				{
					grabSoundPool = ObjectPoolManager.instance.GunGrab02AU;
				}
				else if(data.moduleLevel == 2)
				{
					grabSoundPool = ObjectPoolManager.instance.GunGrab03AU;
				}
				else if(data.moduleLevel == 3)
				{
					grabSoundPool = ObjectPoolManager.instance.GunGrab04AU;
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
