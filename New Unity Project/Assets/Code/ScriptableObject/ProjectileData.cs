using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/Other/ProjectileData", order = 0)]
public class ProjectileData : ScriptableObject {
	
	public float speed;
	public float maxRange;
	public bool impact;
	public float impactForce;
	public int damages;
	public float stun;
	public float gravity;
	public int bounces;
	public int piercing;
	public bool expolsive;
	public int explosiveDamages;
	public float explosiveRange;
	public int explosiveForce;
	public float explosiveStun;
	public bool projectileStickToTarget;
	public LayerMask hitLayer;
	public LayerMask damageLayer;
	public float recoilForce;

}
