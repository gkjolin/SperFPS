using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "IAEyeData", menuName = "ScriptableObjects/Mobs/IAEyeData", order = 2)]
public class IAEyeData : ScriptableObject {
	
	public float halfFOV;
	public float range;
	public LayerMask layer;

}
