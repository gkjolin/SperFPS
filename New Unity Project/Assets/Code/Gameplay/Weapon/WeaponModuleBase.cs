using UnityEngine;
using System.Collections;

public class WeaponModuleBase : MonoBehaviour {

	public WeaponModuleBaseData data;

	[HideInInspector]
	public int currentRafaleCount;
	[HideInInspector]
	public ObjectPool grabSoundPool;

	public void SetUpItem()
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
					grabSoundPool = ObjectPoolManager.instance.GunGrab01_AS;
				}
				else if(data.moduleLevel == 1)
				{
					grabSoundPool = ObjectPoolManager.instance.GunGrab02_AS;
				}
				else if(data.moduleLevel == 2)
				{
					grabSoundPool = ObjectPoolManager.instance.GunGrab03_AS;
				}
				else if(data.moduleLevel == 3)
				{
					grabSoundPool = ObjectPoolManager.instance.GunGrab04_AS;
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
