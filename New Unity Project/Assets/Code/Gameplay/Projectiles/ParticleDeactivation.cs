using UnityEngine;
using System.Collections;

public class ParticleDeactivation : PooledObjectDeactivation {

	private ParticleSystem p;
	private ParticleSystem[] ps;

	protected override void Awake ()
	{
		base.Awake();
		p = GetComponentInChildren<ParticleSystem>();
		ps = GetComponentsInChildren<ParticleSystem>();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		for(int i = 0; i < ps.Length; i++)
		{
			ps[i].randomSeed = (uint)Random.Range(0, 60000);
		}
	}

	void Update () 
	{
		if(p.isPlaying == false && timer <= 0.0f)
		{
			Disable();
		}
	}
}
