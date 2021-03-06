﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flock : MonoBehaviour {

	public float speed = 10f;
	//how fast turn
	float rotationSpeed = 5.0f;
	Vector3 averageHeading;
	Vector3 averagePostion;
	float neighbourDistance = 20.0f;
	float playerDistance;
	float headingDistance;
	float playerDistanceFromFlock;
	// how far the birds will follow the player before turning back this should be in game manager or global flock script
	public float maxPlayerDistanceFromFlock = 60;
	//turn back at end of boundary
	bool turning = false;
	private string filename;
	// Use this for initialization
	void Start () {
		speed = Random.Range(2f,10);

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

		if(turning){
			Vector3 direction = Vector3.zero - transform.position;
			transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(direction),
				rotationSpeed * Time.deltaTime);
			speed = Random.Range(0.5f,2);
		}
		else{

			if(Random.Range(0,6) < 1){
				ApplyRules();
			}
		}

		//move forward
		transform.Translate(0,0, Time.deltaTime * speed);

	}

	void ApplyRules(){

		//get all the birds
		GameObject[] gos;
		gos = globalFlock.allBirds;


		//center of ground
		Vector3 vcenter = Vector3.zero;
		//away from neighbours
		Vector3 vavoid = Vector3.zero;
		//group speed
		float gSpeed = 1f;

		Vector3 headingPos = globalFlock.headingPos;

		playerDistance = Vector3.Distance(GameObject.Find("Player").transform.position, this.transform.position);
		headingDistance = Vector3.Distance(globalFlock.headingPos, this.transform.position);
		playerDistanceFromFlock = GameObject.Find("sceneController").GetComponent<sceneController>().distanceFromFlock;
		// if the player is too far from the heading leave the player
		if(playerDistanceFromFlock < maxPlayerDistanceFromFlock){
			headingPos = GameObject.Find("Player").transform.position;
		}

		float dist;

		int groupSize = 0;
		//avoid other birds
		foreach(GameObject go in gos)
		{
			if(go != this.gameObject)
			{

				dist = Vector3.Distance(go.transform.position, this.transform.position);
				//if within distance we are in a group
				if(dist <= neighbourDistance)
				{
					vcenter += go.transform.position;
					groupSize++;
					//if we are about to collide, too close, we take avoid pos
					if(dist < 10.0f)
					{
						vavoid = vavoid + (this.transform.position - go.transform.position);
					}
					//grab flock script attached to neighbour
					flock anotherFlock = go.GetComponent<flock>();
					// speed of neighbour added to speed
					gSpeed += anotherFlock.speed;
				}

			}


		}


		//if bird is in group, calc avg center and speed
		if(groupSize > 0)
		{
			vcenter = vcenter/groupSize + (headingPos - this.transform.position);
			speed = gSpeed/groupSize;
			//head in direction we need to turn
			Vector3 direction = (vcenter + vavoid*2) - transform.position;
			if(direction != Vector3.zero)
			{
				//Slerp slowly turns us in the direction we are going
				transform.rotation = Quaternion.Slerp(transform.rotation,
					Quaternion.LookRotation(direction),
					rotationSpeed * Time.deltaTime);
			}
		}


	}

}




