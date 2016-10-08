using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour {

	public bool countDmg;
	public int maxLifePoint;

	[HideInInspector]
	public int lifePoint;
	[HideInInspector]
	public bool dead;
	[HideInInspector]
	public bool takeDamage;
	[HideInInspector]
	public float pushBack;
	[HideInInspector]
	public float stuning;
	[HideInInspector]
	public Vector3 damageDirection;
	[HideInInspector]
	public Vector3 damagePosition;

	private bool counting;

	void Start () {
		dead = false;
		lifePoint = maxLifePoint;
	}

	void OnEnable()
	{
		lifePoint = maxLifePoint;
	}

	public void TakeDamage (int damages, float push, float stun, Vector3 direction, Vector3 position) 
	{
		if(counting == false && countDmg == true)
		{
			StartCoroutine(countDamages());
		}
		takeDamage = true;
		pushBack = push;
		stuning = stun;
		damageDirection = direction;
		damagePosition = position;
		lifePoint -= damages;
		if(lifePoint <= 0)
		{
			lifePoint = 0;
			dead = true;
		}
	}

	public void Heal (int d) 
	{
		lifePoint += d;
		if(lifePoint > maxLifePoint)
		{
			lifePoint = maxLifePoint;
			dead = false;
		}
	}

	IEnumerator countDamages()
	{
		counting = true;
		int l = lifePoint;

		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		float value = 0.0f;
		int diff = 0;
		while(value < 10.0f)
		{
			value += Time.deltaTime;
			if(l != lifePoint)
			{
				diff += l - lifePoint;
				l = lifePoint;
			}

			yield return wait;
		}

		Debug.Log(diff/10.0f);

		yield return new WaitForSeconds(2.0f);

		Debug.Log("ready!");
		counting = false;
	}
}
