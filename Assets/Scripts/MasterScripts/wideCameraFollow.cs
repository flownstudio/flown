using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wideCameraFollow : MonoBehaviour {

	public Transform target;
	public Vector3 offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.LookAt(target);

		//TODO: add the bird speed in here. 
		transform.position += transform.forward * Time.deltaTime * 10.0F;
	}
}
