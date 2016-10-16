using UnityEngine;
using System.Collections;

public class MobGenerator : MonoBehaviour {

	public int[] mobTypes;
	public int mobLVLMin;
	public int mobLVLMax;

	public MobGeneratorData data;

	private int mobType;
	private int mobLVL;

	private IABase iaBase;
	private IAWeapon iaWeapon;
	private IAEye iaEye;
	private IAMovement iaMovement;

	private Transform trsf;
	private string mobName;

	void Awake()
	{
		trsf = transform;
		GenerateMob();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.M))
		{
			GenerateMob();
		}
	}

	public void GenerateMob()
	{
		mobType = mobTypes[Random.Range(0, mobTypes.Length)];
		mobLVL = Random.Range(mobLVLMin, mobLVLMax+1);

		GetModules(mobType, mobLVL);

		GameObject baseObject = Instantiate(iaBase.gameObject, trsf.position, trsf.rotation) as GameObject;
		IABase iab = baseObject.GetComponent<IABase>();

		for(int i = 0; i < iab.weaponsSlots.Length; i++)
		{
			Instantiate(iaWeapon.gameObject, iab.weaponsSlots[i].position, iab.weaponsSlots[i].rotation, iab.weaponsSlots[i]);
		}

		for(int i = 0; i < iab.eyeSlots.Length; i++)
		{
			Instantiate(iaEye.gameObject, iab.eyeSlots[i].position, iab.eyeSlots[i].rotation, iab.eyeSlots[i]);
		}

		Instantiate(iaMovement.gameObject, iab.reactorSlot.position, iab.reactorSlot.rotation, iab.reactorSlot);
			
		iab.SetUpIA();

		baseObject.name = mobName + " LVL" + mobLVL;
	}

	void GetModules(int type, int level)
	{
		IABase[] iab;
		IAWeapon[] iaw;
		IAEye[] iae;
		IAMovement[] iam;

		if(type == 0)
		{
			iab = data.IABase_0;
			iaw = data.IAWeapon_0;
			iae = data.IAEye_0;
			iam = data.IAMovement_0;
			mobName = "SphereBot";
		}
		else //Temporary
		{
			iab = data.IABase_0;
			iaw = data.IAWeapon_0;
			iae = data.IAEye_0;
			iam = data.IAMovement_0;
			mobName = "SphereBot";
		}

		int[] r = {Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4)};

		int loopcount = 0;

		while(r[0] + r[1] + r[2] + r[3] != level)
		{
			int i = loopcount%4;
			if(r[0] + r[1] + r[2] + r[3] > level)
			{
				if(r[i]-1 >= 0)
				{
					r[i] -= 1;
				}
			}
			else
			{
				if(r[i]+1 <= 3)
				{
					r[i] += 1;
				}
			}
			loopcount += 1;
		}

		//Debug.Log(r[0].ToString()+r[1].ToString()+r[2].ToString()+r[3].ToString());
		GetBase(iab, r[0]);
		GetWeapon(iaw, r[1]);
		GetEye(iae, r[2]);
		GetReactor(iam, r[3]);
	}

	void GetBase(IABase[] iab, int level)
	{
		for(int i = 0; i < iab.Length; i ++)
		{
			if(iab[i].data.moduleLevel == level)
			{
				iaBase = iab[i];
			}
		}
	}
		
	void GetWeapon(IAWeapon[] iaw, int level)
	{
		for(int i = 0; i < iaw.Length; i ++)
		{
			if(iaw[i].data.moduleLevel == level)
			{
				iaWeapon = iaw[i];
			}
		}
	}

	void GetEye(IAEye[] iae, int level)
	{
		for(int i = 0; i < iae.Length; i ++)
		{
			if(iae[i].data.moduleLevel == level)
			{
				iaEye = iae[i];
			}
		}
	}

	void GetReactor(IAMovement[] iam, int level)
	{
		for(int i = 0; i < iam.Length; i ++)
		{
			if(iam[i].data.moduleLevel == level)
			{
				iaMovement = iam[i];
			}
		}
	}
}
