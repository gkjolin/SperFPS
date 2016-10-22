using UnityEngine;
using System.Collections;

public class InteractableInput : MonoBehaviour {

	private GrabZone grabZone;

	void Awake()
	{
		grabZone = GameObject.FindObjectOfType<GrabZone>();
	}

	void Update () {
		if(Input.GetMouseButtonDown(2))
		{
			if(grabZone.grabableItem != null)
			{
				ButtonSwitch buttonSwitch = grabZone.grabableItem.GetComponent<ButtonSwitch>();
				if(buttonSwitch)
				{
					buttonSwitch.Switch();
				}
			}
		}
	}
}
