using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScriptIsaac : MonoBehaviour {


	public int successRating = 10;

	public float speed = 1.3f;
	float rotationSpeed = 6.0f;

	public float distanceFromFlock;
	public float distanceFromGround;

	public float dangerDistance = 12f;

	bool turning = false;
	bool scoreDirectionUp = true;
	int maxNumOfBirds;

	// Use this for initialization
	void Start () {
		// TODO: get this from game manager or from flock script.
		maxNumOfBirds = 200;
		
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

		distanceFromFlock = Vector3.Distance(globalFlock.headingPos, this.transform.position);
		Vector3 groundPoint = new Vector3(this.transform.position.x, 0, this.transform.position.z);
		distanceFromGround = Vector3.Distance(this.transform.position, groundPoint);

		//move forward
		transform.Translate(0,0, Time.deltaTime * speed);

	}


	public float[] getPlayerInfo(){
		float[] vals = new float[7];
		vals [0] = successRating;
		vals [1] = speed;
		vals [2] = rotationSpeed;
		vals [3] = distanceFromFlock;
		vals [4] = distanceFromGround;
		vals [5] = dangerDistance;
		return vals;
	}

}
