﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int playerID;
	public float stunDuration;
	public float deathForce;
	public float hitWhileJumpingForce;
	public int playerStartingCoins;
	public AnimationCurve stunCurve;
	public Transform respawnPoint;
	public Transform head;
	public Rigidbody rgdBody;
	public PlayerMove playerMove;
	public Damageable damageable;
	public IATarget iaTarget;
	public WeaponInput[] weaponInputs;
	public MouseLook mouseLook;
	public PostProcess postProcess;
	public Collider coll;
	public PlayerSound playerSound;

	[HideInInspector]
	public float playerEquipmentWeight;
	[HideInInspector]
	public int playerCoins;
	[HideInInspector]
	public GenericItem[] genericItems;
	[HideInInspector]
	public bool isAlive;

	void Awake()
	{
		for(int i = 0; i < weaponInputs.Length; i++)
		{
			weaponInputs[i].player = this;
		}
		isAlive = true;

		playerCoins = playerStartingCoins;
	}

	void Start()
	{
		UIManager.instance.Clear();
		UIManager.instance.UpdateLife();
		UIManager.instance.UpdateCoins();
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.P) && postProcess.deathEffectFinished == true)
		{
			Respawn();
		}

		if(damageable.dead == true && isAlive == true)
		{
			isAlive = false;
			Death();
			return;
		}

		if(damageable.takeDamage == true)
		{
			if(damageable.stuning*stunDuration > 0.0f)
			{
				StopAllCoroutines();
				if(playerMove.checkGround.grounded == true)
				{
					StartCoroutine(StunCoroutine(damageable.pushBack, damageable.stuning, damageable.damageDirection));
				}
				else
				{
					rgdBody.AddForce(damageable.pushBack*damageable.damageDirection*hitWhileJumpingForce, ForceMode.VelocityChange);
				}
			}
			damageable.takeDamage = false;
			UIManager.instance.UpdateLife();

			if(postProcess.deathEffectFinished == true)
			{
				postProcess.StopAllCoroutines();
			}

			postProcess.hitPosition = damageable.damagePosition;
			postProcess.StartCoroutine("HitEffect");
			playerSound.PlayHitSound(damageable.damagePosition);
		}

		postProcess.speedEffect = (playerMove.actualSpeed - playerMove.speed)/playerMove.speedBoost;
	}

	public void GetItems()
	{
		genericItems = GetComponentsInChildren<GenericItem>();

		playerEquipmentWeight = 0.0f;
		for(int i = 0; i < genericItems.Length; i++)
		{
			playerEquipmentWeight += genericItems[i].GetComponent<Rigidbody>().mass;
		}
		playerMove.playerEquipmentWeight = playerEquipmentWeight;
		playerMove.SetEquipmentWeight();
	}

	IEnumerator StunCoroutine(float projPushBack, float projStunDuration, Vector3 dir)
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		float value = 0.0f;
		while(value < projStunDuration)
		{
			if(playerMove.checkGround.grounded == false)
			{
				yield break;
			}
			value += Time.deltaTime;
			float ev = stunCurve.Evaluate(value/projStunDuration);
			rgdBody.AddForce(projPushBack*dir*ev, ForceMode.VelocityChange);

			yield return wait;
		}
	}

	void Death()
	{
		postProcess.StartCoroutine("DeathEffect");
		SetUpComponents(false);
		rgdBody.AddForceAtPosition(damageable.pushBack*damageable.damageDirection*deathForce, damageable.damagePosition, ForceMode.VelocityChange);

		coll.material.dynamicFriction = 0.5f;
		coll.material.staticFriction = 0.5f;

		UIManager.instance.UpdateLife();
	}

	void Respawn()
	{
		postProcess.StartCoroutine("RespawnEffect");
		isAlive = true;
		rgdBody.velocity = Vector3.zero;
		rgdBody.angularVelocity = Vector3.zero;
		transform.position = respawnPoint.position;
		transform.rotation = respawnPoint.rotation;
		SetUpComponents(true);

		coll.material.dynamicFriction = 0.0f;
		coll.material.staticFriction = 0.0f;

		damageable.lifePoint = damageable.maxLifePoint;
		damageable.dead = false;

		UIManager.instance.UpdateLife();
	}

	void SetUpComponents(bool b)
	{
		for(int i = 0; i < weaponInputs.Length; i++)
		{
			if(weaponInputs[i].currentGenericItem != null)
			{
				weaponInputs[i].Drop();
			}

			weaponInputs[i].enabled = b;
		}
		rgdBody.freezeRotation = b;
		playerMove.enabled = b;
		mouseLook.enabled = b;
		playerSound.enabled = b;
	}

	void OnCollisionEnter(Collision c)
	{
		if(c.gameObject.layer == LayerMask.NameToLayer("Collectable"))
		{
			Collectable collectable = c.gameObject.GetComponent<Collectable>();
			if((int)collectable.data.collectableType == 0)
			{
				playerCoins += 1;
				playerSound.PlayCollectableSound(0);
				UIManager.instance.UpdateCoins();
			}
			else if((int)collectable.data.collectableType == 1)
			{
				damageable.Heal(1);
				playerSound.PlayCollectableSound(1);
				UIManager.instance.UpdateLife();
			}
			else if((int)collectable.data.collectableType == 2)
			{
				playerMove.Boost();
				playerSound.PlayCollectableSound(2);
			}

			collectable.grab();
		}
	}
}
