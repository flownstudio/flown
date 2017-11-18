using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScriptIsaac : MonoBehaviour {


	public int successRating = 10;

	public float speed = 3.4f;
	float rotationSpeed = 4.0f;

	public float distanceFromFlock;
	public float distanceFromGround;

	public float dangerDistance = 12f;
	private float volumeLevel = 0.0f;
	AudioSource audioSource;
	Vector3 averageHeading;
	Vector3 averagePosition;
	float neighbourDistance = 0.4f;

	bool turning = false;
	bool scoreDirectionUp = true;
	int maxNumOfBirds;

	// Use this for initialization
	void Start () {
		
		// TODO: get this from game manager or from flock script.
		maxNumOfBirds = 200;

		audioSource = gameObject.AddComponent<AudioSource>();

 		audioSource.clip = Resources.Load("isaacsTestResources/Track2Loop - Agitated strings") as AudioClip;

 		audioSource.loop = true;
 		audioSource.volume = volumeLevel;
 		audioSource.spatialBlend = 1.0f;
 		audioSource.Play();

	}


	void Update () {	

		// This is just to test the visibility of other birds adding them in
		// one at a time.
		// 1% chance this frame adds a bird up to max birds then goes back down
		if(UnityEngine.Random.Range(0,100) < 1){
			if(scoreDirectionUp){
				successRating += 2;
				if(successRating == maxNumOfBirds){
					scoreDirectionUp = false;
				}
			}
			else{
				successRating -= 2;
				if(successRating == 0){
					scoreDirectionUp = true;
				}
			}
		}

		//UPDATE AUDIO
		distanceFromFlock = Vector3.Distance(globalFlock.headingPos, this.transform.position);
		Vector3 groundPoint = new Vector3(this.transform.position.x, 0, this.transform.position.z);
		distanceFromGround = Vector3.Distance(this.transform.position, groundPoint);

		if(distanceFromFlock < dangerDistance){
			volumeLevel = 0.0f;
		}
		else{
			volumeLevel = (distanceFromFlock - dangerDistance) * 0.02f;
		}
		
		audioSource.volume = volumeLevel;



		//UPDATE HEADING
		if(Vector3.Distance(transform.position, Vector3.zero) >= globalFlock.sceneSize)
		{
			turning = true;
		}
		else{
			turning = false;
		}

		if(turning){
			Vector3 direction = Vector3.zero - transform.position;
			transform.rotation = Quaternion.Slerp(transform.rotation,
													Quaternion.LookRotation(direction),
													rotationSpeed * Time.deltaTime);
			speed = UnityEngine.Random.Range(0.5f,2);
		}
		else{

			if(UnityEngine.Random.Range(0,6) < 1){
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
		float gSpeed = 0.1f;

		Vector3 headingPos = globalFlock.headingPos;

		float dist;

		int groupSize = 0;
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
					if(dist < 2f)
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
			Vector3 direction = (vcenter + vavoid) - transform.position;
			if(direction != Vector3.zero)
			{
				//Slerp slowly turns us in the direction we are going
				transform.rotation = Quaternion.Slerp(transform.rotation,
													Quaternion.LookRotation(direction),
													rotationSpeed * Time.deltaTime);
			}
		}


	}

	public float[] playerInfo(){
		float[] vals = new float[7];
		vals [0] = successRating;
		vals [1] = speed;
		vals [2] = rotationSpeed;
		vals [3] = distanceFromFlock;
		vals [4] = distanceFromGround;
		vals [5] = dangerDistance;
		vals [6] = volumeLevel;
		return vals;
	}

}
