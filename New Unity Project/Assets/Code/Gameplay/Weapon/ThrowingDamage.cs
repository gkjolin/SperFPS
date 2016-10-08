using UnityEngine;
using System.Collections;

public class ThrowingDamage : MonoBehaviour {

	public ThrowingDamageData throwingDamageData;

	private Rigidbody rgdBody;

	void Awake()
	{
		rgdBody = GetComponent<Rigidbody>();
	}
		
	void OnCollisionEnter(Collision c)
	{
		if(throwingDamageData.damageLayer == (throwingDamageData.damageLayer | (1 << c.gameObject.layer)))
		{
			float v = rgdBody.velocity.sqrMagnitude;
			if(v > throwingDamageData.squaredVelocityThreshold)
			{
				Damageable dam = c.transform.GetComponentInChildren<Damageable>();
				if(dam)
				{
					dam.TakeDamage(throwingDamageData.throwingDamage, throwingDamageData.impactForce, throwingDamageData.throwingStun, rgdBody.velocity.normalized, c.contacts[0].point);
				}
			}
		}
	}
}
