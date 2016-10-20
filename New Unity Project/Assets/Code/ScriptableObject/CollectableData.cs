using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "CollectableData", menuName = "ScriptableObjects/Collectable/CollectableData", order = 0)]
public class CollectableData : ScriptableObject {

	public enum CollectableType
	{
		Coin = 0,
		Heal = 1,
		Speed = 2,
	};

	public CollectableType collectableType;
	public float duration;
	public AnimationCurve animationrCurve;
	public float animSpeed;
	public float initialSpeed;
	public float rotationSpeed;

}
