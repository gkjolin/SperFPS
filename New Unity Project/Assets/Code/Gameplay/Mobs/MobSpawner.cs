using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MobSpawner : MonoBehaviour {

	public int number;
	public int[] mobTypes;
	public int minLevel;
	public int maxLevel;
	public float radius;
	public float minDelay;
	public float maxDelay;

	private MobGenerator mobGenerator;
	private Transform trsf;
	private Coroutine currentSpawnCoroutine;

	void Awake()
	{
		trsf = transform;
		mobGenerator = GetComponent<MobGenerator>();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.M))
		{
			Spawn ();
		}
	}

	public void Spawn () {
		if(currentSpawnCoroutine != null)
		{
			StopCoroutine(currentSpawnCoroutine);
		}
		currentSpawnCoroutine = StartCoroutine(SpawnCoroutine());
	}

	IEnumerator SpawnCoroutine()
	{
		for(int i = 0; i < number; i++)
		{
			yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
			GameObject newMob = mobGenerator.GenerateMob(Random.Range(0, mobTypes.Length), Random.Range(minLevel, maxLevel+1));
			Vector2 randomCircle = Random.insideUnitCircle*radius;
			Vector3 randomPosition = transform.position + new Vector3(randomCircle.x, 1.0f, randomCircle.y);
			newMob.transform.position = randomPosition;
			MobWaveGenerator.instance.mobList.Add(newMob);
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1.0f,0.0f,0.0f,1.0f);
		Gizmos.DrawWireSphere(trsf.position, radius);
	}
}
