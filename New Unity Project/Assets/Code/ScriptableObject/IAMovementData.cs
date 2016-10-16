using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "IAMovementData", menuName = "ScriptableObjects/Mobs/IAMovementData", order = 4)]
public class IAMovementData : ScriptableObject {

	public int moduleLevel;

	public float speed;
	public float acceleration;
	public float orientationSpeed;
	public float reactionTime;
	public bool canStrafe;
	public Vector2 strafeFrequency;
	public Vector2 strafeSpeed;
	public Vector2 strafeDuration;
	public float startStrafingDisance;
	public AnimationCurve strafeCurve;

}
