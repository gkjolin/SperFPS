using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunShop : MonoBehaviour {

	private enum GunShopStats
	{
		available = 0,
		processing = 1,
		done = 2,
		unavailable = 3
	}

	public WeaponGenerator weaponGenerator;
	public ButtonSwitch buyButton;
	public ButtonSwitch priceUpButton;
	public ButtonSwitch priceDownButton;
	public float priceChangeSpeed;
	public float processingTime;
	public int lvlMinCost;
	public int lvlMaxCost;
	public Text price;
	public Text potentialWeaponLVL;

	private GunShopStats gunShopStats;
	private int currentPrice;
	private bool changingPrice;
	private bool processing;
	private GenericInteractable buyGenericInteractable;
	private GenericInteractable priceUpGenericInteractable;
	private GenericInteractable priceDownGenericInteractable;
	private int maxCost;

	void Awake()
	{
		buyGenericInteractable = buyButton.GetComponent<GenericInteractable>();
		priceUpGenericInteractable = priceUpButton.GetComponent<GenericInteractable>();
		priceDownGenericInteractable = priceDownButton.GetComponent<GenericInteractable>();
	}

	void Start () {
		gunShopStats = GunShopStats.available;
		currentPrice = lvlMaxCost;
		maxCost = CalculateMaxCost();
		changingPrice = false;
		processing = false;
		weaponGenerator.weaponLVLMin = 0;
		weaponGenerator.weaponLVLMax = 0;
		InitializeUI();
	}

	void Update () {
		SwitchStats();
	}

	void SwitchStats()
	{
		switch ((int)gunShopStats)
		{
		case 0 :
			{
				Shopping();
				break;
			}
		case 1 :
			{
				if(processing == false)
				{
					StartCoroutine(Processing());
				}
				break;
			}
		case 2 :
			{
				break;
			}
		case 3 :
			{
				break;
			}
		}
	}

	void InitializeUI()
	{
		UpdateUI();
	}

	void Shopping()
	{
		if(priceUpButton.onOff == true && changingPrice == false)
		{
			StartCoroutine(PriceChange(CalculateAmount()));
		}
		if(priceDownButton.onOff == true && changingPrice == false)
		{
			StartCoroutine(PriceChange(-CalculateAmount()));
		}
		if(buyButton.onOff == true)
		{
			buyButton.onOff = false;
			if(GameManager.instance.players[0].playerCoins >= currentPrice && currentPrice >= lvlMaxCost)
			{
				gunShopStats = GunShopStats.processing;
				buyGenericInteractable.interactable = false;
				priceUpGenericInteractable.interactable = false;
				priceDownGenericInteractable.interactable = false;
			}
		}
	}

	int CalculateAmount()
	{
		return 10;
	}

	int CalculateMaxCost()
	{
		return 10000;
	}

	IEnumerator PriceChange(int amount)
	{
		changingPrice = true;
		if((currentPrice < maxCost && amount > 0) || (currentPrice > lvlMaxCost && amount < 0))
		{
			currentPrice += amount;
			weaponGenerator.weaponLVLMin = Mathf.Clamp(currentPrice/lvlMinCost, 0, 9);
			weaponGenerator.weaponLVLMax = Mathf.Clamp((currentPrice - lvlMaxCost)/lvlMaxCost, 0, 9);
			UpdateUI();
		}
		yield return new WaitForSeconds(priceChangeSpeed);
		changingPrice = false;
	}

	IEnumerator Processing()
	{
		processing = true;
		GameManager.instance.players[0].playerCoins -= currentPrice;
		UIManager.instance.UpdateCoins();
		yield return new WaitForSeconds(processingTime);
		weaponGenerator.GenerateWeapon();
		gunShopStats = GunShopStats.available;
		buyGenericInteractable.interactable = true;
		priceUpGenericInteractable.interactable = true;
		priceDownGenericInteractable.interactable = true;
		processing = false;
	}

	void UpdateUI()
	{
		price.text = currentPrice + " $";
		potentialWeaponLVL.text = weaponGenerator.weaponLVLMin + " - " + weaponGenerator.weaponLVLMax;
	}

}
