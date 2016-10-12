using UnityEngine;
using System.Collections;

public class ProjectilePlayer : ProjectileMovement {

	public GameManager.ProjectileType projectileType;

	protected override void SetUpImpact()
	{
		switch(projectileType)
		{
		case GameManager.ProjectileType.Gun :
			{
				impactStandardPool = ObjectPoolManager.instance.gunImpact;
				impactHitPool = ObjectPoolManager.instance.gunImpactHit;
				break;
			}
		case GameManager.ProjectileType.Laser :
			{
				impactStandardPool = ObjectPoolManager.instance.laserImpact;
				break;
			}
		case GameManager.ProjectileType.Dart :
			{
				impactStandardPool = ObjectPoolManager.instance.dartImpact;
				break;
			}
		}
	}
}
