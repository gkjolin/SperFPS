using UnityEngine;
using System.Collections;

public class SoundObjectDeactivation : PooledObjectDeactivation {

	private AudioSource audioSrc;

	protected override void Awake ()
	{
		base.Awake();
		audioSrc = GetComponent<AudioSource>();
	}

	void Update () 
	{
		if(audioSrc.isPlaying == false && timer <= 0.0f)
		{
			Disable();
		}
	}
}
