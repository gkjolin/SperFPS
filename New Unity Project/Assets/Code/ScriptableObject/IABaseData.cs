using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "IABaseData", menuName = "ScriptableObjects/Mobs/IABaseData", order = 1)]
public class IABaseData : ScriptableObject {

	public int moduleLevel;

	public GameManager.MobType mobType;

	public int maxLifePoint;
	public int dropCount;
	public float coinProbability;
	public float healProbability;
	public float speedProbability;
	public bool canMoveWhileShooting;
	public float hearing;
	public Vector2 searchingDistance;
	public Vector2 searchingTimeRange;
	public float stunDuration;
	public float pushBack;
	public AnimationCurve pushCurve;
	public float deathDuration;
	public float deathPushBack;
	public float spawnDuration;
	public float moveSoundVolume;
	public float moveSoundPitch;
	public float moveSoundMaxSpeed;

}
