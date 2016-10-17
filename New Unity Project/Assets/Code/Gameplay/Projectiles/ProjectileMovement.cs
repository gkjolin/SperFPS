using UnityEngine;
using System.Collections;

public class ProjectileMovement : MonoBehaviour {

	public ProjectileData data;

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
		bounceCount = data.bounces;
		piercingCount = data.piercing;
		StartCoroutine(DisableAfterDelay());
	}

	void FixedUpdate()
	{
		if(initialForce == true)
		{
			initialForce = false;
			rgdBody.AddForce(trsf.forward*data.speed, ForceMode.VelocityChange);
		}

		rgdBody.AddForce(Vector3.up*data.gravity, ForceMode.Acceleration);

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

		//Debug.DrawRay(ray.origin, trsf.forward*data.speed*Time.fixedDeltaTime);
			
		CheckColision(ray);
	}

	void CheckColision(Ray r)
	{
		if (Physics.Raycast(r, out hit, data.speed*Time.fixedDeltaTime, data.hitLayer))
		{
			GameObject impactFx = impactStandardPool.GetCurrentPooledGameObject();

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
				if(data.impact && hit.collider.gameObject.transform.parent)
				{
					Rigidbody hitRgdBody = hit.collider.gameObject.transform.parent.GetComponent<Rigidbody>();
					if(hitRgdBody)
					{
						hitRgdBody.AddForceAtPosition(trsf.forward*data.impactForce, hit.point, ForceMode.Impulse);
					}
				}

				//DAMAGE EFFECTS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				if(data.damageLayer == (data.damageLayer | (1 << hit.collider.gameObject.layer)))				
				{
					Damageable dam = hit.collider.gameObject.GetComponent<Damageable>();
					if(dam)
					{
						dam.TakeDamage(data.damages, data.impactForce, data.stun, trsf.forward, hit.point);
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
			if(data.expolsive == true)
			{
				Collider[] hitedColliders = Physics.OverlapSphere(hit.point, data.explosiveRange, data.hitLayer);
				for(int i = 0; i < hitedColliders.Length; i++)
				{
					Transform hitedTransform = hitedColliders[i].transform;
					Vector3 dir = (hitedTransform.position - trsf.position).normalized;
					Rigidbody hitRgdBody = hitedColliders[i].gameObject.transform.GetComponent<Rigidbody>();

					if(hitRgdBody)
					{
						hitRgdBody.AddForce(dir*data.explosiveForce, ForceMode.Impulse);
					}

					if(data.damageLayer == (data.damageLayer | (1 << hitedColliders[i].gameObject.layer)))
					{
						Damageable dam = hitedColliders[i].gameObject.GetComponent<Damageable>();
						if(dam)
						{
							dam.TakeDamage(data.explosiveDamages, data.explosiveForce, data.explosiveStun, dir, hitedTransform.position);
						}
					}
				}
			}

			//FXs/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			FXUtilities.instance.SetFx(impactFx, hit.collider.transform, hit.point, hit.normal, false);
		}
	}

//	void SetFx(GameObject go, RaycastHit h)
//	{
//		if(go)
//		{
//			if(data.projectileStickToTarget)
//			{
//				go.transform.parent = h.collider.transform;
//				go.transform.forward = -trsf.forward;
//			}
//			else
//			{
//				go.transform.forward = h.normal;
//			}
//
//			go.transform.position = h.point;
//			go.SetActive(true);
//		}
//		else
//		{
//			Debug.Log("Neen More Chicken!");
//		}
//	}

	protected virtual void SetUpImpact()
	{

	}

	IEnumerator DisableAfterDelay()
	{
		yield return new WaitForSeconds(data.maxRange/data.speed);
		gameObject.SetActive(false);
	}
		
	void OnDisable()
	{
		rgdBody.velocity = Vector3.zero;
		StopAllCoroutines();
	}
}
