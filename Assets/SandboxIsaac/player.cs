using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

	public float distanceFromFlock;
	public float distanceFromGround;

	public double frequency = 440;
	public double gain = 0.05;

	private double increment;
	private double phase;
	private double sampling_frequency = 48000;


	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {		
		distanceFromFlock = Vector3.Distance(globalFlock.headingPos, this.transform.position);
		Vector3 groundPoint = new Vector3(this.transform.position.x, 0, this.transform.position.z);
		distanceFromGround = Vector3.Distance(this.transform.position, groundPoint);
	}

	// UNCOMMENT TO MAKE SOUND
	// void OnAudioFilterRead(float[] data, int channels)
	// {
	// 	// update increment in case frequency has changed
	// 	increment = frequency * distanceFromFlock * Math.PI / sampling_frequency;

	// 	for (var i = 0; i < data.Length; i = i + channels)
	// 	{
	// 		phase = phase + increment;
	// 		// this is where we copy audio data to make them “available” to Unity
	// 		data[i] = (float)(gain*Math.Sin(phase));
	// 		// if we have stereo, we copy the mono data to each channel
	// 		if (channels == 2) data[i + 1] = data[i];
	// 		if (phase > 2 * Math.PI) phase = 0;
	// 	}
	// }

}
