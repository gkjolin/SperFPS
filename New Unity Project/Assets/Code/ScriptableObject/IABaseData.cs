using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "IABaseData", menuName = "ScriptableObjects/Mobs/IABaseData", order = 1)]
public class IABaseData : ScriptableObject {

	public int moduleLevel;

	public int maxLifePoint;
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

}
