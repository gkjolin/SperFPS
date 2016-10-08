using UnityEngine;
using System.Collections;

public class GenericWeapon : GenericItem {

	[HideInInspector]
	public WeaponModuleBase weaponModuleBase;
	[HideInInspector]
	public WeaponModuleMagazine weaponModuleMagazine;
	[HideInInspector]
	public WeaponModuleCannon weaponModuleCannon;

	private CameraShake cameraShake;
	private float rate1;
	private float rate2;

	public override void SetUpItem()
	{
		base.SetUpItem();
		weaponModuleBase = GetComponent<WeaponModuleBase>();
		weaponModuleMagazine = GetComponentInChildren<WeaponModuleMagazine>();
		weaponModuleCannon = GetComponentInChildren<WeaponModuleCannon>();
		cameraShake = GameObject.FindObjectOfType<CameraShake>() as CameraShake;
		rgdBody.mass = weaponModuleBase.data.mass + weaponModuleMagazine.data.mass + weaponModuleCannon.data.mass * weaponModuleCannon.cannons.Length;

		rate1 = weaponModuleBase.data.rafaleRate*weaponModuleMagazine.data.fireRateLimiter;
		rate2 = weaponModuleBase.data.fireRate*weaponModuleMagazine.data.fireRateLimiter*(1.0f + (weaponModuleCannon.cannons.Length-1.0f)*0.5f);
	}
		
	public override void Use()
	{
		if(used == false)
		{
			if(weaponModuleMagazine.currentMagazine > 0)
			{
				StartCoroutine(FireSequence());
			}
			else
			{
				StartCoroutine(ReloadSequence());
			}
		}
	}

	public override void Reload()
	{
		if(used == false && weaponModuleMagazine.currentMagazine < weaponModuleMagazine.data.magazine)
		{
			StartCoroutine(ReloadSequence());
		}
	}

//	public override void Drop(float force, float torque)
//	{
//		base.Drop(force, torque);
//	}

	public override void Take(WeaponInput w)
	{
		base.Take(w);
		GameObject takeGO = weaponModuleBase.grabSoundPool.GetCurrentPooledGameObject();
		SetSound(takeGO, trsf);
	}

	IEnumerator FireSequence()
	{
		used = true;

		while(weaponModuleBase.currentRafaleCount > 0)
		{
			if(weaponModuleMagazine.currentMagazine <= 0)
			{
				break;
			}

			weaponModuleBase.currentRafaleCount -= 1;

			if(cameraShake)
			{
				cameraShake.amplitude = weaponModuleMagazine.data.shakeAmplitude*weaponModuleCannon.cannons.Length;
				cameraShake.frequency = weaponModuleMagazine.data.shakeFrequency;
				cameraShake.duration = weaponModuleMagazine.data.shakeDuration;
				cameraShake.SetAmplitude();
			}

			GameObject shootGO = weaponModuleCannon.shootSoundPool.GetCurrentPooledGameObject();
			SetSound(shootGO, trsf);

			noise = weaponModuleCannon.data.weaponNoise;

			player.rgdBody.AddForce(trsf.forward * -weaponModuleMagazine.recoilForce, ForceMode.VelocityChange);

			GameObject[] projectils = new GameObject[weaponModuleCannon.cannons.Length];

			for(int i = 0; i < weaponModuleCannon.cannons.Length; i++)
			{
				if(weaponModuleMagazine.currentMagazine > 0)
				{
					GameObject fireFx = weaponModuleMagazine.fireFXPool.GetCurrentPooledGameObject();
					SetFx(fireFx, weaponModuleCannon.cannons[i].transform);

					projectils[i] = weaponModuleMagazine.projectilsPool.GetCurrentPooledGameObject();

					projectils[i].transform.position = weaponModuleCannon.cannons[i].transform.position;

					Vector3 rot = weaponModuleCannon.cannons[i].transform.rotation.eulerAngles;
					rot.x += Random.Range(-weaponModuleCannon.data.precision,weaponModuleCannon.data.precision);
					rot.y += Random.Range(-weaponModuleCannon.data.precision,weaponModuleCannon.data.precision);
					rot.z += Random.Range(-weaponModuleCannon.data.precision,weaponModuleCannon.data.precision);
					projectils[i].transform.rotation = Quaternion.Euler(rot);

					projectils[i].SetActive(true);
					weaponModuleMagazine.currentMagazine -= 1;
				}
			}

			UpdateUI(false);

			float value = 0.0f;

			WaitForEndOfFrame wait = new WaitForEndOfFrame();

			float rafaleRate;
			if(weaponModuleBase.currentRafaleCount > 0)
			{
				rafaleRate = rate1;
			}
			else
			{
				rafaleRate = rate2;
			}

			while(value < rafaleRate)
				{
					float ev = weaponModuleBase.data.weaponFireCurve.Evaluate(value/rafaleRate);
					if(rightHand)
					{
						trsf.localPosition = ev*weaponModuleBase.data.weaponRecoilPosition;
						trsf.localRotation = Quaternion.AngleAxis(ev*weaponModuleBase.data.weaponRecoilRotation, weaponModuleBase.data.weaponRecoilRotationAxis);
					}
					else
					{
						float pX = weaponModuleBase.data.weaponRecoilPosition.x * -1.0f;
						trsf.localPosition = ev*new Vector3(pX, weaponModuleBase.data.weaponRecoilPosition.y, weaponModuleBase.data.weaponRecoilPosition.z);
						float rX = weaponModuleBase.data.weaponRecoilRotationAxis.x * -1.0f;
						trsf.localRotation = Quaternion.AngleAxis(ev*weaponModuleBase.data.weaponRecoilRotation * -1.0f, new Vector3(rX, weaponModuleBase.data.weaponRecoilRotationAxis.y, weaponModuleBase.data.weaponRecoilRotationAxis.z));
					}
					value += Time.deltaTime;
					yield return wait;
				}
			}

		trsf.localPosition = Vector3.zero;
		trsf.localRotation = Quaternion.identity;

		weaponModuleBase.currentRafaleCount = weaponModuleBase.data.rafaleCount;
		noise = 0.0f;

		used = false;
	}

	IEnumerator ReloadSequence()
	{
		used = true;
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		float value = 0.0f;
		bool reloadSoundPlayed = false;

		while(value < weaponModuleMagazine.data.reloadTime)
		{
			if(value >= weaponModuleMagazine.data.reloadSoundDelay && reloadSoundPlayed == false)
			{
				reloadSoundPlayed = true;
				GameObject reloadGO = weaponModuleMagazine.reloadSoundPool.GetCurrentPooledGameObject();
				SetSound(reloadGO, trsf);
			}

			float ev = weaponModuleBase.data.weaponReloadCurve.Evaluate(value/weaponModuleMagazine.data.reloadTime);
			if(rightHand)
			{
				trsf.localPosition = ev*weaponModuleBase.data.weaponReloadPosition;
				trsf.localRotation = Quaternion.AngleAxis(ev*weaponModuleBase.data.weaponReloadRotation, weaponModuleBase.data.weaponReloadRotationAxis);	
			}
			else
			{
				float pX = weaponModuleBase.data.weaponReloadPosition.x * -1.0f;
				trsf.localPosition = ev*new Vector3(pX, weaponModuleBase.data.weaponReloadPosition.y, weaponModuleBase.data.weaponReloadPosition.z);
				float rX = weaponModuleBase.data.weaponReloadRotationAxis.x * -1.0f;
				trsf.localRotation = Quaternion.AngleAxis(ev*weaponModuleBase.data.weaponReloadRotation * -1.0f, new Vector3(rX, weaponModuleBase.data.weaponReloadRotationAxis.y, weaponModuleBase.data.weaponReloadRotationAxis.z));
			}
			value += Time.deltaTime;
			yield return wait;
		}

		trsf.localPosition = Vector3.zero;
		trsf.localRotation = Quaternion.identity;

		weaponModuleBase.currentRafaleCount = weaponModuleBase.data.rafaleCount;
		weaponModuleMagazine.currentMagazine = weaponModuleMagazine.data.magazine;
		UpdateUI(false);
		used = false;
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

	public override void UpdateUI(bool empty)
	{
		base.UpdateUI(empty);
		UIManager.instance.UpdateMagazine(weaponModuleMagazine.data.magazine, weaponModuleMagazine.currentMagazine, rightHand, empty);
	}
}
