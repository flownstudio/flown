using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSounds : MonoBehaviour {

	public float dangerDistance;
	public float distanceFromFlock;
	private float volumeLevel = 0.0f;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		
		audioSource = gameObject.AddComponent<AudioSource>();
 		audioSource.clip = Resources.Load("isaacsTestResources/Track2Loop - Agitated strings") as AudioClip;
 		audioSource.loop = true;
 		audioSource.volume = volumeLevel;
 		audioSource.spatialBlend = 1.0f;
 		audioSource.Play();

	}

	// Update is called once per frame
	void Update () {		
		float[] feedbackArray = this.GetComponent<MouseControlMegan>().getCamMouseAngleDiff();
		distanceFromFlock = feedbackArray[1];

		// Creating sounds for a range
		//IF too close sound
		//IF too far sound
		//DO the sliding scale/fading between sounds.

		//ANOTHER SCRIPT
		//DO the same idea/scale for post-processing image.
		//Can we put arrows?? 
		//SHADER on camera etc


		if(distanceFromFlock < dangerDistance){
			volumeLevel = 0.0f;
		}
		else{
			volumeLevel = (distanceFromFlock - dangerDistance) * 0.1f;
		}
		
		audioSource.volume = volumeLevel;
	}

}
