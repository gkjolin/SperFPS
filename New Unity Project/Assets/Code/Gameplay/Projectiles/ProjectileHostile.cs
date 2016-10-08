using UnityEngine;
using System.Collections;

public class ProjectileHostile : ProjectileMovement {

	public GameManager.HostileProjectileType hostileProjectileType;

	protected override void SetUpImpact()
	{
		switch(hostileProjectileType)
		{
		case GameManager.HostileProjectileType.HostileBullet :
			{
				impactStandardPool = ObjectPoolManager.instance.hostileBulletImpact;
				impactHitPool = ObjectPoolManager.instance.hostileBulletImpact;
				break;
			}
		case GameManager.HostileProjectileType.HostileLaser :
			{
				break;
			}
		}
	}
}
