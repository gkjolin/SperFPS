using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour {

	public AudioSource leftStepSound;
	public AudioSource rightStepSound;
	public AudioSource jumpSound;
	public AudioSource landSound;
	public AudioSource movingSound;
	public AudioSource hitSound;
	public AudioSource[] collectableSounds;
	public Rigidbody rgdBody;
	public PlayerMove playerMove;
	public Player player;
	public float movingSoundVolume;
	public float movingSoundPitch;
	public float stepFrequency;

	private bool playingStepSound = false;
	private bool playingJumpSound = false;
	private bool playingLandSound = false;
	private bool switchStep;
	private SoundRandomizer leftStepSoundRandomizer;
	private SoundRandomizer rightStepSoundRandomizer;
	private SoundRandomizer jumpSoundRandomizer;
	private SoundRandomizer landSoundRandomizer;
	private SoundRandomizer hitSoundRandomizer;
	private SoundRandomizer[] collectableSoundRandomizers;

	void Awake()
	{
		leftStepSoundRandomizer = leftStepSound.GetComponent<SoundRandomizer>();
		rightStepSoundRandomizer = rightStepSound.GetComponent<SoundRandomizer>();
		jumpSoundRandomizer = jumpSound.GetComponent<SoundRandomizer>();
		landSoundRandomizer = landSound.GetComponent<SoundRandomizer>();
		hitSoundRandomizer = hitSound.GetComponent<SoundRandomizer>();
		collectableSoundRandomizers = new SoundRandomizer[collectableSounds.Length];
		for(int i = 0; i < collectableSounds.Length; i++)
		{
			collectableSoundRandomizers[i] = collectableSounds[i].GetComponent<SoundRandomizer>();
		}

		stepFrequency *= Mathf.PI;
	}

	void Update () {
		if(playerMove.moving == true && playingStepSound == false)
		{
			StartCoroutine(stepSoundCoroutine());
		}

		if(playerMove.jumping == true && playingJumpSound == false)
		{
			StartCoroutine(jumpSoundCoroutine());
		}

		if(playerMove.checkGround.grounded == true && playingLandSound == false)
		{
			playingLandSound = true;
			landSoundRandomizer.Randomize();
			landSound.Play();
		}

		if(playerMove.checkGround.grounded == false)
		{
			playingLandSound = false;
		}
			
		float v = Mathf.Clamp(rgdBody.velocity.magnitude, 0.0f, 10.0f);
		movingSound.volume = movingSoundVolume*v;
		movingSound.pitch = movingSoundPitch*v;
	}

	public void PlayHitSound(Vector3 p)
	{
		hitSoundRandomizer.Randomize();
		hitSound.transform.position = p;
		hitSound.Play();
	}

	public void PlayCollectableSound(int i)
	{
		collectableSoundRandomizers[i].Randomize();
		collectableSounds[i].Play();
	}

	IEnumerator stepSoundCoroutine()
	{
		playingStepSound = true;

		if(switchStep == true)
		{
			leftStepSoundRandomizer.Randomize();
			leftStepSound.Play();
		}
		else
		{
			rightStepSoundRandomizer.Randomize();
			rightStepSound.Play();
		}

		switchStep = !switchStep;
		yield return new WaitForSeconds(stepFrequency/playerMove.actualSpeed);
		playingStepSound = false;
	}

	IEnumerator jumpSoundCoroutine()
	{
		playingJumpSound = true;
		jumpSoundRandomizer.Randomize();
		jumpSound.Play();
		yield return new WaitForSeconds(0.1f);
		playingJumpSound = false;
	}
}
