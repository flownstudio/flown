using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdSounds : MonoBehaviour {

	private string filename;

	// Use this for initialization
	void Start () {

		AudioSource audioSource = gameObject.AddComponent<AudioSource>();

		int filenumber = Random.Range(1,4);
		switch(filenumber){
			case 1:
				filename = "isaacsTestResources/one";
				break;
			case 2:
				filename = "isaacsTestResources/two";
				break;
			case 3:
				filename = "isaacsTestResources/three";
				break;
			case 4:
				filename = "isaacsTestResources/four";
				break;
		}
 		audioSource.clip = Resources.Load(filename) as AudioClip;
 		audioSource.loop = true;
 		audioSource.volume = 0.05f;
 		audioSource.spatialBlend = 1.0f;

 		audioSource.PlayDelayed(Random.Range(0.0f,1));
	}
	
	// Update is called once per frame
	void Update () {


	}

	

}




