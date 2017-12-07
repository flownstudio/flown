using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCollisionReporter : MonoBehaviour {

	public Collider collisionCollider;
	public GameObject collisionGameObject;
	public Transform collisionTransform;
	public bool isColliding;

	public Collider triggerCollider;
	public Rigidbody triggerRigidbody;
	public Bounds triggerBounds;
	public bool isTriggered;

	void start(){
		isColliding = false;
		isTriggered = false;

	}
	// TRIGGERS
	// Trigger events are only sent if one of the Colliders also has a Rigidbody attached. 
	// OnTriggerEnter is called when the Collider other enters the trigger.
    void OnTriggerEnter(Collider other) {
		isTriggered = true;
		triggerCollider = other;
		triggerRigidbody = other.attachedRigidbody;
		triggerBounds = other.bounds;
    }

    // OnTriggerExit is called when the Collider other has stopped touching the trigger.
    void OnTriggerStay(Collider other){


    }

    // OnTriggerStay is called almost all the frames for every Collider other that is touching the trigger. 
    // The function is on the physics timer so it won't necessarily run every frame.
     void OnTriggerExit(Collider other){
		isTriggered = false;
    }


    // COLLISIONS
    // With normal, non-trigger collisions, there is an additional detail that at least one of the objects 
    // involved must have a non-kinematic Rigidbody (ie, Is Kinematic must be switched off). 
    // If both objects are kinematic Rigidbodies then OnCollisionEnter, etc, will not be called. 
    // With trigger collisions, this restriction doesn’t apply and so both kinematic and non-kinematic 
    // Rigidbodies will prompt a call to OnTriggerEnter when they enter a trigger collider.

    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
    void OnCollisionEnter(Collision collision)
    {
    	isColliding = true;
    	collisionCollider = collision.collider;
    	collisionGameObject = collision.gameObject;
    	collisionTransform = collision.transform;
    }
    
    //OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.
    void OnCollisionStay(Collision collision)
    {

    }

    //OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
    void OnCollisionExit(Collision collision)
    {
    	isColliding = false;
    }


}
