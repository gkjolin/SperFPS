using UnityEngine;
using System.Collections;

public class CollectableGenerator : MonoBehaviour {

	private Transform trsf;
	private ObjectPool coinPool;
	private ObjectPool healPool;
	private ObjectPool speedPool;

	void Awake () {
		trsf = transform;
		coinPool = ObjectPoolManager.instance.coin;
		healPool = ObjectPoolManager.instance.heal;
		speedPool = ObjectPoolManager.instance.speed;
	}

	public void SpawnCollectables(int number, float coinProbability, float healProbability, float speedProbability)
	{
		float coinAndHealProbability = coinProbability + healProbability;
		float totalProbability = coinAndHealProbability + speedProbability;
		for(int i = 0; i < number; i++)
		{
			float r = Random.Range(0.0f, totalProbability);

			//Coin
			if(r < coinProbability)
			{
				GameObject coin = coinPool.GetCurrentPooledGameObject() as GameObject;
				SpawnUtilities.instance.SetGOPosition(coin, trsf, false);
			}
			//Heal
			else if(r >= coinProbability && r < coinAndHealProbability)
			{
				GameObject heal = healPool.GetCurrentPooledGameObject() as GameObject;
				SpawnUtilities.instance.SetGOPosition(heal, trsf, false);
			}
			//Speed
			else if(r >= coinAndHealProbability && r < totalProbability)
			{
				GameObject speed = speedPool.GetCurrentPooledGameObject() as GameObject;
				SpawnUtilities.instance.SetGOPosition(speed, trsf, false);
			}
		}
	}
}
