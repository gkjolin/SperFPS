using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour {

	public AudioSource leftStepSound;
	public AudioSource rightStepSound;
	public AudioSource jumpSound;
	public AudioSource landSound;
	public AudioSource movingSound;
	public Rigidbody rgdBody;
	public PlayerMove playerMove;
	public CheckGround checkGround;
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

	void Awake()
	{
		leftStepSoundRandomizer = leftStepSound.GetComponent<SoundRandomizer>();
		rightStepSoundRandomizer = rightStepSound.GetComponent<SoundRandomizer>();
		jumpSoundRandomizer = jumpSound.GetComponent<SoundRandomizer>();
		landSoundRandomizer = landSound.GetComponent<SoundRandomizer>();
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

		if(checkGround.grounded == true && playingLandSound == false)
		{
			playingLandSound = true;
			landSoundRandomizer.Randomize();
			landSound.Play();
		}

		if(checkGround.grounded == false)
		{
			playingLandSound = false;
		}

		float v = rgdBody.velocity.magnitude;
		movingSound.volume = movingSoundVolume*v;
		movingSound.pitch = movingSoundPitch*v;
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
		yield return new WaitForSeconds(stepFrequency);
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
