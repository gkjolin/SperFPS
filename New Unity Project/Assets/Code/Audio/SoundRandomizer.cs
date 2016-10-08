using UnityEngine;
using System.Collections;

public class SoundRandomizer : MonoBehaviour {

	public bool playOnEnable = true;
	public bool useRandomClip;
	public AudioClip[] audioClips;
	public float volumeMultiplier;
	public float pitchMultiplier;
	public Vector2 randomVolume;
	public Vector2 randomPitch;
	private AudioSource audioSrc;

	void Awake () {
		audioSrc = GetComponent<AudioSource>();
	}

	void OnEnable () {
		if(playOnEnable)
		{
			Randomize();
		}
	}

	public void Randomize()
	{
		if(useRandomClip)
		{
			audioSrc.clip = audioClips[Random.Range(0, audioClips.Length)];
		}
		audioSrc.volume = Random.Range(randomVolume.x, randomVolume.y)*volumeMultiplier;
		audioSrc.pitch = Random.Range(randomPitch.x, randomPitch.y)*pitchMultiplier;
		audioSrc.Play();
	}
}
