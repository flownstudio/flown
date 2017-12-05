using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wideCameraFollow : MonoBehaviour {

	public Transform target;
	public Vector3 offset;
	public GameObject sceneController;
	private float birdspeed;

	// Use this for initialization
	void Start () {
		birdspeed = sceneController.GetComponent<sceneController> ().playerSpeed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.LookAt(target);

		transform.position += transform.forward * Time.deltaTime * birdspeed/2;
	}
}
