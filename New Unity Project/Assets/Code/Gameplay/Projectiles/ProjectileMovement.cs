using UnityEngine;
using System.Collections;

public class ProjectileMovement : MonoBehaviour {

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
	public string[] hitLayer;
	public string[] damageLayer;

	protected Transform trsf;
	protected Rigidbody rgdBody;
	protected RaycastHit hit;
	protected ObjectPool impactStandardPool;
	protected ObjectPool impactHitPool;

	private bool initialForce;
	private bool bounce;
	private int bounceCount;
	private int piercingCount;
	private Vector3 reflectVector = Vector3.zero;

	void Awake()
	{
		trsf = transform;
		rgdBody = GetComponent<Rigidbody>();
		SetUpImpact();
	}

	void OnEnable()
	{
		initialForce = true;
		bounce = false;
		bounceCount = bounces;
		piercingCount = piercing;
		StartCoroutine(DisableAfterDelay());
	}

	void FixedUpdate()
	{
		if(initialForce == true)
		{
			initialForce = false;
			rgdBody.AddForce(trsf.forward*speed, ForceMode.VelocityChange);
		}

		rgdBody.AddForce(Vector3.up*gravity, ForceMode.Acceleration);

		if(rgdBody.velocity != Vector3.zero)
		{
			trsf.forward = rgdBody.velocity.normalized;
		}

		Ray ray;
		if(bounce == true)
		{
			bounce = false;
			initialForce = true;
			trsf.forward = reflectVector;

			ray = new Ray(rgdBody.position, reflectVector);
		}
		else
		{
			ray = new Ray(rgdBody.position, trsf.forward);
		}

		Debug.DrawRay(ray.origin, ray.direction*speed*Time.fixedDeltaTime);
			
		CheckColision(ray);
	}

	void CheckColision(Ray r)
	{
		LayerMask layer = LayerMask.GetMask(hitLayer);
		if (Physics.Raycast(r, out hit, speed*Time.fixedDeltaTime, layer))
		{
			GameObject impactFx = impactStandardPool.GetCurrentPooledGameObject();
			LayerMask dmgLayer = LayerMask.GetMask(damageLayer);

			//STANDARD EFFECTS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if(hit.collider.gameObject.layer != 0)
			{
				//PIERCING EFFECTS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				if(piercingCount > 0)
				{
					piercingCount -= 1;
				}
				else
				{
					gameObject.SetActive(false);
				}

				//IMPACT EFFECTS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				if(impact && hit.collider.gameObject.transform.parent)
				{
					Rigidbody hitRgdBody = hit.collider.gameObject.transform.parent.GetComponent<Rigidbody>();
					if(hitRgdBody)
					{
						hitRgdBody.AddForceAtPosition(trsf.forward*impactForce, hit.point, ForceMode.Impulse);
					}
				}

				//DAMAGE EFFECTS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				if(dmgLayer == (dmgLayer | (1 << hit.collider.gameObject.layer)))				
				{
					Damageable dam = hit.collider.gameObject.GetComponent<Damageable>();
					if(dam)
					{
						dam.TakeDamage(damages, impactForce, stun, trsf.forward, hit.point);
					}

					impactFx = impactHitPool.GetCurrentPooledGameObject();
				}
			}
			//BOUNCE EFFECTS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			else if(bounceCount > 0)
			{
				reflectVector = Vector3.Reflect(trsf.forward, hit.normal).normalized;
				rgdBody.velocity = Vector3.zero;
				rgdBody.position = hit.point;
				bounceCount -= 1;
				bounce = true;
			}
			else
			{
				gameObject.SetActive(false);
			}

			//EXPLOSIVE DAMAGES/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if(expolsive == true)
			{
				Collider[] hitedColliders = Physics.OverlapSphere(hit.point, explosiveRange, layer);
				for(int i = 0; i < hitedColliders.Length; i++)
				{
					Transform hitedTransform = hitedColliders[i].transform;
					Vector3 dir = (hitedTransform.position - trsf.position).normalized;
					Rigidbody hitRgdBody = hitedColliders[i].gameObject.transform.parent.GetComponent<Rigidbody>();

					if(hitRgdBody)
					{
						hitRgdBody.AddForce(dir*explosiveForce, ForceMode.Impulse);
					}

					if(dmgLayer == (dmgLayer | (1 << hitedColliders[i].gameObject.layer)))
					{
						Damageable dam = hitedColliders[i].gameObject.GetComponent<Damageable>();
						if(dam)
						{
							dam.TakeDamage(explosiveDamages, explosiveForce, explosiveStun, dir, hitedTransform.position);
						}
					}
				}
			}

			//FXs/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			SetFx(impactFx, hit);
		}
	}

	void SetFx(GameObject go, RaycastHit h)
	{
		if(go)
		{
			if(projectileStickToTarget)
			{
				go.transform.parent = h.collider.transform;
				go.transform.forward = -trsf.forward;
			}
			else
			{
				go.transform.forward = h.normal;
			}

			go.transform.position = h.point;
			go.SetActive(true);
		}
		else
		{
			Debug.Log("Neen More Chicken!");
		}
	}

	protected virtual void SetUpImpact()
	{

	}

	IEnumerator DisableAfterDelay()
	{
		yield return new WaitForSeconds(maxRange/speed);
		gameObject.SetActive(false);
	}
		
	void OnDisable()
	{
		rgdBody.velocity = Vector3.zero;
		StopAllCoroutines();
	}
}
