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
	public ObjectPool sphereBot_Death;
	public ObjectPool sphereBot_Spawn;

	[Space(20)]
	public ObjectPool Drop01_AS;
	[Space(10)]
	public ObjectPool GunReload01_AS;
	public ObjectPool GunReload02_AS;
	public ObjectPool GunReload03_AS;
	public ObjectPool GunReload04_AS;
	public ObjectPool GunGrab01_AS;
	public ObjectPool GunGrab02_AS;
	public ObjectPool GunGrab03_AS;
	public ObjectPool GunGrab04_AS;
	public ObjectPool GunShotx1_AS;
	public ObjectPool GunShotx2_AS;
	public ObjectPool GunShotx3_AS;
	public ObjectPool GunShotx4_AS;
	[Space(10)]
	public ObjectPool SphereBot_AS;
	public ObjectPool SphereBotHit_AS;

	void Awake () {
		instance = this;
	}
}
