using UnityEngine;
using System.Collections;

public class InteractableInput : MonoBehaviour {

	private GrabZone grabZone;
	private ButtonSwitch buttonSwitch;

	void Awake()
	{
		grabZone = GameObject.FindObjectOfType<GrabZone>();
	}

	void Update () {
		if(Input.GetMouseButton(2))
		{
			if(grabZone.grabableItem != null)
			{
				if(buttonSwitch == null)
				{
					buttonSwitch = grabZone.grabableItem.GetComponent<ButtonSwitch>();
				}
				if(buttonSwitch)
				{
					buttonSwitch.interacting = true;
					buttonSwitch.Switch();
				}
			}
			else
			{
				if(buttonSwitch)
				{
					buttonSwitch.interacting = false;
					buttonSwitch = null;
				}
			}
		}

		if(Input.GetMouseButtonUp(2))
		{
			if(buttonSwitch)
			{
				buttonSwitch.interacting = false;
				buttonSwitch = null;
			}
		}
	}
}
