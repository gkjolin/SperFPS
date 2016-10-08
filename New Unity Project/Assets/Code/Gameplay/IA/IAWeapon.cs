using UnityEngine;
using System.Collections;

public class IAWeapon : MonoBehaviour {

	public IAWeaponData data;

	public Transform[] cannons;

	[HideInInspector]
	public bool used;
	[HideInInspector]
	public bool targetDetected;
	[HideInInspector]
	public float distanceFromPlayer;
	[HideInInspector]
	public float reactionTime;

	private Vector3 averageCannonPosition;
	private ObjectPool projectilsPool;
	private bool inRange;
	private ProjectileMovement projectileMovment;
	private bool startFiring;
	private bool fire;
	private Transform trsf;

	void Awake()
	{
		trsf = transform;
		if(data.avoidFriendlyFire)
		{
			averageCannonPosition = Vector3.zero;
			for(int i = 0; i < cannons.Length; i++)
			{
				averageCannonPosition += cannons[i].localPosition;
			}
			averageCannonPosition /= (float)cannons.Length;
		}

		projectilsPool = GameObject.Find(data.hostileProjectileType.ToString()).GetComponent<ObjectPool>();
		projectileMovment = projectilsPool.pooledObject.GetComponent<ProjectileMovement>();
		used = false;
		startFiring = false;
		fire = false;
	}

	void Update()
	{
		if(projectileMovment.maxRange*projectileMovment.maxRange > distanceFromPlayer)
		{
			inRange = true;
		}
		else
		{
			inRange = false;			
		}

		if(inRange == true && startFiring == false && targetDetected == true)
		{
			StartCoroutine(DelayBeforeFire());
		}
	}

	IEnumerator DelayBeforeFire()
	{
		startFiring = true;
		used = true;

		yield return new WaitForSeconds(reactionTime);

		Ray ray = new Ray(trsf.TransformPoint(averageCannonPosition), trsf.forward);

		if(Physics.Raycast(ray, projectileMovment.maxRange, data.friendlyLayer))
		{
			used = false;
			yield return new WaitForSeconds(Random.Range(data.attackFrequency.x, data.attackFrequency.y));
			startFiring = false;
			yield break;
		}
			
		int shotCount = Random.Range(data.minShot, data.maxShot+1);
		int i = 1;
		while(i <= shotCount)
		{
			i++;
			fire = true;
			StartCoroutine(FireSequence());
			yield return new WaitUntil(() => fire == false);
		}
		used = false;
		yield return new WaitForSeconds(Random.Range(data.attackFrequency.x, data.attackFrequency.y));
		startFiring = false;
	}

	IEnumerator FireSequence()
	{
		GameObject[] projectils = new GameObject[cannons.Length];
		for(int i = 0; i < cannons.Length; i++)
		{
			int hostileProjectileTypeInt = (int)data.hostileProjectileType;
			switch (hostileProjectileTypeInt)
				{
				case 0:
					{
						GameObject fireFx = ObjectPoolManager.instance.hostileBulletFire.GetCurrentPooledGameObject();
						if(fireFx)
						{
							SetFx(fireFx, cannons[i].transform);
						}
						break;
					}
				}

			projectils[i] = projectilsPool.GetCurrentPooledGameObject();
			projectils[i].transform.position = cannons[i].transform.position;

			Vector3 rot = cannons[i].transform.rotation.eulerAngles;
			rot.x += Random.Range(-data.precision,data.precision);
			rot.y += Random.Range(-data.precision,data.precision);
			rot.z += Random.Range(-data.precision,data.precision);
			projectils[i].transform.rotation = Quaternion.Euler(rot);

			projectils[i].SetActive(true);
		}

		float value = 0.0f;
		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		while(value < data.fireRate)
		{
			value += Time.deltaTime;
			yield return wait;
		}
			
		fire = false;

		yield return null;
	}

	void SetFx(GameObject go, Transform t)
	{
		if(go)
		{
			go.transform.position = t.position;
			go.transform.forward = t.forward;
			go.SetActive(true);
			go.transform.SetParent(t);
		}
		else
		{
			Debug.Log("Neen More Chicken!");
		}
	}
	void OnDisable()
	{
		StopAllCoroutines();
		used = false;
		fire = false;
		startFiring = false;
	}
}
