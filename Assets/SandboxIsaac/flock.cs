using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flock : MonoBehaviour {

	public float speed = 2f;
	//how fast turn
	float rotationSpeed = 4.0f;

	Vector3 averageHeading;
	Vector3 averagePostion;
	float neighbourDistance = 4.0f;

	//turn back at end of boundary
	bool turning = false;

	// Use this for initialization
	void Start () {
		speed = Random.Range(0.5f,2);
	}
	
	// Update is called once per frame
	void Update () {

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

}




