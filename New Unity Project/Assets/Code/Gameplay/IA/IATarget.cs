using UnityEngine;
using System.Collections;

public class IATarget : MonoBehaviour {

	public float walkingSoundRange;
	[HideInInspector]
	public float soundProduced;
	[HideInInspector]
	public Vector3 position;
	[HideInInspector]
	public Transform parent;
	[HideInInspector]
	public Player player;

	private Transform trsf;

	void Awake () {
		trsf = transform;
		parent = trsf.parent;
		player = parent.GetComponent<Player>();
	}
	
	void Update()
	{
		position = trsf.position;

		if(player)
		{
			float weaponNoise = 0.0f;
			for(int i = 0 ; i < player.genericItems.Length; i++)
			{
				weaponNoise += player.genericItems[i].noise;
			}

			if(player.playerMove.moving == true)
			{
				soundProduced = walkingSoundRange + weaponNoise;
			}
			else
			{
				soundProduced = weaponNoise;
			}
		}
	}
}
