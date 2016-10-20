using UnityEngine;
using System.Collections;

public class GenericItem : MonoBehaviour {

	public GenericItemData genericItemData;

	[HideInInspector]
	public float noise;
	[HideInInspector]
	public bool inHand;
	[HideInInspector]
	public bool used;
	[HideInInspector]
	public WeaponInput weaponInput;
	[HideInInspector]
	public Player player;
	[HideInInspector]
	public bool rightHand;
	[HideInInspector]
	public Collider grabCollider;
	[HideInInspector]
	public Transform parent;

	protected Transform trsf;

	protected Rigidbody rgdBody;
	private Collider[] colls;
	private Renderer[] renderers;
	private Material sharedMaterial;

	public virtual void SetUpItem()
	{
		trsf = transform;
		rgdBody = GetComponent<Rigidbody>();
		colls = GetComponentsInChildren<Collider>();
		renderers = GetComponentsInChildren<Renderer>();
		sharedMaterial = renderers[0].sharedMaterial;
		inHand = false;

		for(int i = 0; i < colls.Length; i++)
		{
			if(colls[i].gameObject.layer == LayerMask.NameToLayer("WeaponGrab"))
			{
				grabCollider = colls[i];
				break;
			}
		}

		parent = GameManager.instance.itemGroup;
		trsf.SetParent(parent);
	}

	public virtual void Use()
	{
	}

	public virtual void Reload()
	{
	}

	public virtual void Drop(float force, float torque)
	{
		GameObject dropGO = ObjectPoolManager.instance.Drop01_AS.GetCurrentPooledGameObject();
		SpawnUtilities.instance.SetGOPosition(dropGO, trsf, false);
		StopAllCoroutines();
		inHand = false;
		UpdateUI(true);
		weaponInput.currentGenericItem = null;
		weaponInput.itemInHand = false;
		weaponInput = null;
		SetPhysics(inHand);
		trsf.SetParent(parent);
		rgdBody.AddForce(trsf.forward*force, ForceMode.VelocityChange);
		rgdBody.AddTorque(trsf.right*torque, ForceMode.VelocityChange);
		player.GetItems();
	}

	public virtual void Take(WeaponInput w)
	{
		inHand = true;
		SetPhysics(inHand);
		SetRenderers(false);
		weaponInput = w;
		trsf.SetParent(w.transform);
		SetPhysics(inHand);
		player.GetItems();
		StartCoroutine(GrabSequence());
	}

	public virtual void UpdateUI(bool empty)
	{
		UIManager.instance.UpdateWeapon(gameObject.name, rightHand, empty);
	}

	public void SetPhysics(bool b)
	{
		rgdBody.isKinematic = b;
		for(int i = 0; i < colls.Length; i++)
		{
			colls[i].enabled = !b;
		}
	}

	public void SetRenderers(bool grab)
	{
		if(grab)
		{
			for(int i = 0; i < renderers.Length; i++)
			{
				renderers[i].material = genericItemData.hightLightMaterial;
			}
		}
		else
		{
			for(int i = 0; i < renderers.Length; i++)
			{
				renderers[i].material = sharedMaterial;
			}
		}
	}

	IEnumerator GrabSequence()
	{
		used = true;
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		float value = 0.0f;

		while(value < genericItemData.grabTime)
		{
			float ev = genericItemData.weaponGrabCurve.Evaluate(value/genericItemData.grabTime);
			trsf.localPosition = ev*genericItemData.weaponGrabPosition;
			trsf.localRotation = Quaternion.AngleAxis(ev*genericItemData.weaponGrabRotation, genericItemData.weaponGrabRotationAxis);
			value += Time.deltaTime;
			yield return wait;
		}

		trsf.localPosition = Vector3.zero;
		trsf.localRotation = Quaternion.identity;

		used = false;
	}
}
