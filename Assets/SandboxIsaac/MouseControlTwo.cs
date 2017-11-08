using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControlTwo : MonoBehaviour {
	//Public variables
	public GameObject target;//This is the invisible target point on the path or whatever
	public GameObject birdPlayer;

	private float speed = 1.0F;
	private GameObject cameraContainer;
	private Quaternion rot;
	private float compareAngle; 

	// Use this for initialization
	void Start () {
		cameraContainer = new GameObject ("Camera Container");

		//transforms cameraContainer to camera position
		cameraContainer.transform.position = transform.position;
		//Sets cameraConainer as parent of camera
		transform.SetParent (cameraContainer.transform);
	
	}
		
	private void Update(){
		float step = speed * Time.deltaTime;
		float primeDistance = 10;// ideal distance to be flying behind the point

		//Calculate the distance between the camera and the point the sphere/bird needs to follow
		float distance = Vector3.Distance (target.transform.position, transform.position);

		Debug.Log(distance);

		//compare the angle/distance between mouse and point on path.
		CompareAngle ();





		//Beginning of the Feedback
		//Using compareAngle we can play around with a bunch of things, link it to Audio etc. see below functions
		//
		//Need way more sophisticated forces/speeds/accelerating etc.
		//Maybe way to link the point on path ??
		if (compareAngle <= 4 && distance >= primeDistance) {
			speed += 0.1F;
		} else if (compareAngle > 4) {
			speed -= 0.3F;
		} else {
			speed -= 0.05F;
		}






			
		//Transform camera toward target unless we are at primeDistance behind point.
		if (distance >= primeDistance) {
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, step);
			//transform.position = Vector3.Lerp(transform.position, target.transform.position, step);// LErp seems to be weird tes with real point
		}
	}

	private void CompareAngle(){
		Vector3 worldPoint,relativePos;
		Camera cam = GetComponent<Camera>();
		Ray ray;// mouse Vector3 ray into real space 
		Quaternion directLook, mouseLook;

		//get ray of mouse to calculate real point in space
		ray = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);// draw ray for now

		//get direction of point on path in relation to camera
		relativePos = target.transform.position - transform.position;

		//get direction of point on path in relation to camera offset by mouse ray direction/point
		worldPoint = (target.transform.position + ray.direction) - transform.position;

		//get the two orientation/directions
		directLook = Quaternion.LookRotation(relativePos);
		mouseLook = Quaternion.LookRotation (worldPoint);	

		//apply the mouseLook orienation to the camera
		transform.localRotation = mouseLook;

		//Compare the Quaternions by getting their angle between two
		compareAngle = Quaternion.Angle(mouseLook, directLook);

//		Debug.Log(compareAngle);
	}

	//Figure out if this is callable/storable in other scripts.
	public float getCamMouseAngleDiff(){
		return compareAngle;
	}

}
	