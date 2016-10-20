using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public int playerID;
	public static UIManager instance;

	public Text magazineLeft;
	public Text magazineRight;
	public Text lifePoints;
	public Text coins;
	public Text frameRate;
	public Text weaponLeft;
	public Text weaponRight;

	private Player player;
	private float moyFrame;
	private float frameCount;
	private string noWeaponString = "no weapon";

	void Awake () {
		instance = this;
		player = GameManager.instance.players[playerID];
		moyFrame = 0.0f;
		frameCount = 0f;
	}

	void Update()
	{
		frameCount += 1f;
		float f = 1.0f / Time.deltaTime;
		moyFrame += f;
		int fint = (int)f;
		int moyFrameInt = (int)(moyFrame/frameCount);
		frameRate.text = fint.ToString() + " / " + moyFrameInt.ToString();
	}

	public void Clear()
	{
		magazineLeft.text = "";
		magazineRight.text = "";
		weaponLeft.text = noWeaponString;
		weaponRight.text = noWeaponString;
	}

	public void UpdateMagazine(int magazine, int currentMagazine, bool rightHand, bool empty)
	{
		if(rightHand == true)
		{
			if(empty == true)
			{
				magazineRight.text = "";
			}
			else
			{
				magazineRight.text = currentMagazine.ToString();
			}
		}
		else
		{
			if(empty == true)
			{
				magazineLeft.text = "";
			}
			else
			{
				magazineLeft.text = currentMagazine.ToString();
			}
		}
	}

	public void UpdateLife()
	{
		lifePoints.text = player.damageable.lifePoint.ToString();
	}

	public void UpdateWeapon(string weaponName, bool rightHand, bool empty)
	{
		if(rightHand)
		{
			if(empty)
			{
				weaponRight.text = noWeaponString;
			}
			else
			{
				weaponRight.text = weaponName;
			}
		}
		else
		{
			if(empty)
			{
				weaponLeft.text = noWeaponString;
			}
			else
			{
				weaponLeft.text = weaponName;
			}
		}
	}

	public void UpdateCoins()
	{
		coins.text = player.playerCoins.ToString();
	}
}
