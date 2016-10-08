using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Player[] players;

	public enum ProjectileType
	{
		Gun = 0,
		Laser = 1,
		Dart = 2,
	};

	public enum HostileProjectileType
	{
		HostileBullet = 0,
		HostileLaser = 1,
	};

	public static GameManager instance;

	void Awake () {
		instance = this;
		for(int i = 0; i < players.Length; i++)
		{
			players[i].playerID = i;
		}
	}
}
