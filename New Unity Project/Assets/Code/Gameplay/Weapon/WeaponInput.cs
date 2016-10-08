using UnityEngine;
using System.Collections;

public class WeaponInput : MonoBehaviour {

	public bool rightHand;
	[SerializeField]
	private float dropForce = 2000.0f;
	[SerializeField]
	private float moveDropForce = 2000.0f;
	[SerializeField]
	private float dropTorque = 2000.0f;

	[HideInInspector]
	public bool itemInHand;
	[HideInInspector]
	public GenericItem currentGenericItem;
	[HideInInspector]
	public Player player;

	private Transform trsf;
	private GrabZone grabZone;

	void Awake()
	{
		trsf = transform;
		grabZone = GameObject.FindObjectOfType<GrabZone>();
	}

	void Start () {
		GetCurrentGenericItem();
	}

	void Update () 
	{
		if(rightHand)
		{
			if(Input.GetMouseButton(1))
			{
				if(itemInHand == true)
				{
					currentGenericItem.Use();
				}
				else
				{
					Grab();
				}
			}
			if(Input.GetAxis("DropRightHand") > 0.5f && itemInHand == true && currentGenericItem.used == false)
			{
				Drop();
			}
		}
		else
		{
			if(Input.GetMouseButton(0))
			{
				if(itemInHand == true)
				{
					currentGenericItem.Use();
				}
				else
				{
					Grab();
				}
			}
			if(Input.GetAxis("DropLeftHand") > 0.5f && itemInHand == true && currentGenericItem.used == false)
			{
				Drop();
			}
		}
			
		if(Input.GetAxis("Reload") > 0.5f)
		{
			if(itemInHand == true)
			{
				currentGenericItem.Reload();
			}
		}
	}

	void Grab()
	{
		currentGenericItem = grabZone.grabableItem;
		if(currentGenericItem && currentGenericItem.inHand == false)
		{
			grabZone.RemoveFromLists(currentGenericItem);
			SetCurrentGenericItem();
		}
	}

	public void Drop()
	{
		currentGenericItem.Drop(dropForce*(1.0f + Vector3.Dot(player.playerMove.rgdBody.velocity, trsf.forward)*moveDropForce), dropTorque);
		player.GetItems();
	}

	public void GetCurrentGenericItem()
	{
		currentGenericItem = GetComponentInChildren<GenericItem>();

		if(currentGenericItem)
		{
			SetCurrentGenericItem();
		}
		else
		{
			itemInHand = false;
		}
	}

	void SetCurrentGenericItem()
	{
		currentGenericItem.player = player;
		currentGenericItem.Take(this);
		itemInHand = true;
		currentGenericItem.rightHand = rightHand;
		currentGenericItem.UpdateUI(false);
		player.GetItems();
	}
}
