using UnityEngine;
using System.Collections;

public class ObjectPoolManager : MonoBehaviour {

	public static ObjectPoolManager instance;
	public ObjectPool gun;
	public ObjectPool dart;
	public ObjectPool laser;
	public ObjectPool hostileBullet;
	[Space(20)]

	public ObjectPool gunImpact;
	public ObjectPool gunImpactHit;
	public ObjectPool gunFire;
	public ObjectPool laserImpact;
	public ObjectPool laserFire;
	public ObjectPool dartImpact;
	public ObjectPool dartFire;

	[Space(20)]
	public ObjectPool hostileBulletFire;
	public ObjectPool hostileBulletImpact;

	[Space(20)]
	public ObjectPool sphereBot_LVL0_Death;

	[Space(20)]
	public ObjectPool Drop01AU;
	public ObjectPool GunReload01AU;
	public ObjectPool GunReload02AU;
	public ObjectPool GunReload03AU;
	public ObjectPool GunReload04AU;
	public ObjectPool GunGrab01AU;
	public ObjectPool GunGrab02AU;
	public ObjectPool GunGrab03AU;
	public ObjectPool GunGrab04AU;
	public ObjectPool GunShotx1AU;
	public ObjectPool GunShotx2AU;
	public ObjectPool GunShotx3AU;
	public ObjectPool GunShotx4AU;

	void Awake () {
		instance = this;
	}
}
