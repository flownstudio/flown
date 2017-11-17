using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	//Public variables
	public GameObject target;//This is the invisible target point on the path or whatever
	public Camera attachedCamera;// Model that we are putting in front of camera

	private float speed = 1.0F;
//	private GameObject cameraContainer;
	private Quaternion rot;
	private float compareAngle, distance;

	private bool gyroEnabled;
	private Gyroscope gyro;

	// Use this for initialization
	void Start () {
//		cameraContainer = new GameObject ("Camera Container");
		Cursor.visible = false;

		//transforms cameraContainer to camera position
//		cameraContainer.transform.position = transform.position;
		//Sets cameraConainer as parent of camera
//		transform.SetParent (cameraContainer.transform);

		gyroEnabled = EnableGyro();
	}

	private bool EnableGyro(){
		if(SystemInfo.supportsGyroscope)
		{
			gyro = Input.gyro;
			gyro.enabled = true;

//			cameraContainer.transform.rotation = Quaternion.Euler (90f, 90f, 0f);
//			rot = new Quaternion (0, 0, 1, 0);

			return true;
		}
		return false;
	}

	private void Update(){

		//MEGAN
		//in update play around with directional control instead
		// maybe also taking speed/gravity/aceleration/deccelration into account

		// tap or space bar flaps the birds wings.
		// three taps max 
		// directional up and down for moving up or down using mouse and gyro
		// do the camera adjust positon
		// the bird axis rotates
		// the widening of camera angle


		float primeDistance = 5;// ideal distance to be flying behind the point

		//Calculate the distance between the camera and the point the sphere/bird needs to follow
		distance = Vector3.Distance (target.transform.position, transform.position);

		float step = speed * Time.deltaTime;


		Debug.Log("DISTANCE: " + distance);


		//compare the angle/distance between mouse and point on path.
		CompareAngle ();

		Vector3 transformTarget = target.transform.position;

		//Beginning of the Feedback
		//Using compareAngle we can play around with a bunch of things, link it to Audio etc. see below functions
		//
		//Need way more sophisticated forces/speeds/accelerating etc.
		//Maybe way to link the point on path ??
		if (compareAngle <= 5.0) {
			if (distance > primeDistance) {
				speed += (float) 0.01*(distance-primeDistance);
			} else {
				speed -= 0.001F;
			}
		} else if (compareAngle > 5.0) {
			speed -= 1.0F;
		} 



		//The problem is that the speed needs to be set by the speed of the bird.
		//get to the point in the transform and then start to deccelerate.

		Debug.Log("SPEED: " + speed);

		//Transform camera toward target unless we are at primeDistance behind point.
		if (distance >= primeDistance) {
			transform.position = Vector3.MoveTowards (transform.position, transformTarget, step);
			//transform.position = Vector3.Lerp(transform.position, target.transform.position, step);// LErp seems to be weird tes with real point
		}
	}

	private void CompareAngle(){

		//BECKY
		//Give directional look angle too...
		//create direction too 
		// 



		Vector3 worldPoint,relativePos;
//		Camera cam = GetComponent<Camera>();
//		Camera cam = transform.GetChild(1);
		Ray ray;// mouse Vector3 ray into real space 
		Quaternion directLook, playerLook;

		//get direction of point on path in relation to camera
		relativePos = target.transform.position - transform.position;

		//get the two orientation/directions
		directLook = Quaternion.LookRotation(relativePos);
	

		//apply the mouseLook orienation to the camera
		if (gyroEnabled) {
//			directLook *= rot;

			Quaternion referenceRotation = directLook;
			Quaternion deviceRotation = gyro.attitude;
			Quaternion eliminationOfXY = Quaternion.Inverse(
				Quaternion.FromToRotation(referenceRotation * Vector3.up, 
					deviceRotation * Vector3.up)
			);
			Quaternion rotationZ = eliminationOfXY * deviceRotation;
			playerLook = rotationZ;

//			playerLook = gyro.attitude *  directLook;//

		} else {
			//get ray of mouse to calculate real point in space
			ray = attachedCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);// draw ray for now

			//get direction of point on path in relation to camera offset by mouse ray direction/point
			worldPoint = (target.transform.position + (ray.direction*3)) - transform.position;

			playerLook = Quaternion.LookRotation (worldPoint);

		}

		transform.localRotation = playerLook;

		//Compare the Quaternions by getting their angle between two
		compareAngle = Quaternion.Angle (playerLook, directLook);

		//How to compare the

		//		Debug.Log("ANGLE: " + compareAngle);
		//		Debug.Log("relativePos: " + relativePos);
	}

	//Figure out if this is callable/storable in other scripts.
	public float[] getCamMouseAngleDiff(){
		float[] vals = new float[2];
		vals [0] = compareAngle;
		vals [1] = distance;
		return vals;
	}

	void OnGUI() {
		string gui = "ANGLE: " + compareAngle;
		GUI.Label(new Rect(10, 10, 500, 20), gui );
	}

}
