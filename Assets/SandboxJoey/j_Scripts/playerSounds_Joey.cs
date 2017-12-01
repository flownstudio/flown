using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class playerSounds_Joey : MonoBehaviour {

	public Swipe swipeControls;

	public float tooFarDistance;
	public float tooCloseDistance;
	public float distanceFromFlock;
	private float tooFarVolumeLevel = 0.0f;
	AudioSource tooFarAudioSource;
	private float tooCloseVolumeLevel = 0.0f;
	AudioSource tooCloseAudioSource;
	private float justRightVolumeLevel = 0.0f;
	AudioSource justRightAudioSource;
	AudioSource flapAudioSource;

	// Use this for initialization
	void Start () {

		tooFarAudioSource = gameObject.AddComponent<AudioSource>();
 		tooFarAudioSource.clip = Resources.Load("isaacsTestResources/cone") as AudioClip;
 		tooFarAudioSource.loop = true;
 		tooFarAudioSource.volume = tooFarVolumeLevel;
 		tooFarAudioSource.spatialBlend = 1.0f;
 		//tooFarAudioSource.Play();

 		tooCloseAudioSource = gameObject.AddComponent<AudioSource>();
 		tooCloseAudioSource.clip = Resources.Load("isaacsTestResources/ctwo") as AudioClip;
 		tooCloseAudioSource.loop = true;
 		tooCloseAudioSource.volume = tooCloseVolumeLevel;
 		tooCloseAudioSource.spatialBlend = 1.0f;
 		//tooCloseAudioSource.Play();

 		justRightAudioSource = gameObject.AddComponent<AudioSource>();
 		justRightAudioSource.clip = Resources.Load("isaacsTestResources/cthree") as AudioClip;
 		justRightAudioSource.loop = true;
 		justRightAudioSource.volume = justRightVolumeLevel;
 		justRightAudioSource.spatialBlend = 1.0f;
 		//justRightAudioSource.Play();

		flapAudioSource = gameObject.AddComponent<AudioSource>();
		flapAudioSource.clip = Resources.Load("flap3") as AudioClip;
		flapAudioSource.loop = true;
		flapAudioSource.volume = 1f;
		//flapAudioSource.spatialBlend = 1.0f;
		//flapAudioSource.Play();

	}

	// Update is called once per frame
	void Update () {
		if(swipeControls.Tap)
		{
			flapAudioSource.Play();
		}

		float distanceFromFlock = this.GetComponent<sceneController>().distanceFromFlock;

		if(distanceFromFlock < tooCloseDistance){
			tooCloseVolumeLevel = tooCloseDistance - distanceFromFlock; //increases
			justRightVolumeLevel = distanceFromFlock * 0.1f; //decreases
			tooFarVolumeLevel = 0.0f; //nothing
		}else if(distanceFromFlock < tooFarDistance && distanceFromFlock > tooCloseDistance){
			tooCloseVolumeLevel = 0.0f; //nothing
			justRightVolumeLevel = 0.9f; //middle
			tooFarVolumeLevel = 0.0f; //nothing
		}
		else{
			tooCloseVolumeLevel = 0.0f; //nothing
			justRightVolumeLevel = distanceFromFlock * -0.1f; //decreases
			tooFarVolumeLevel = (distanceFromFlock - tooFarDistance) * 0.1f;//increases
		}

		tooCloseAudioSource.volume = tooCloseVolumeLevel;
		justRightAudioSource.volume = justRightVolumeLevel;
		tooFarAudioSource.volume = tooFarVolumeLevel;


	}



}
