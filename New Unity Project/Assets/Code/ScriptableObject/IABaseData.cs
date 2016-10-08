using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "IABaseData", menuName = "ScriptableObjects/Mobs/IABaseData", order = 1)]
public class IABaseData : ScriptableObject {

	public int maxLifePoint;
	public float orientationSpeed;
	public float reactionTime;
	public bool canMoveWhileShooting;
	public float hearing;
	public Vector2 searchingDistance;
	public Vector2 searchingTimeRange;
	public bool canStrafe;
	public Vector2 strafeFrequency;
	public Vector2 strafeSpeed;
	public Vector2 strafeDuration;
	public float startStrafingDisance;
	public AnimationCurve strafeCurve;
	public float stunDuration;
	public float pushBack;
	public AnimationCurve pushCurve;
	public float deathDuration;
	public float deathForceMultiplier;

}
