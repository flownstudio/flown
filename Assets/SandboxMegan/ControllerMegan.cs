using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMegan : MonoBehaviour {
	//Public variables
	public GameObject target;
	public Camera attachedCamera;// Model that we are putting in front of camera
	public GameObject birdModel;
	public Vector3 spaceBuffer;

	private float speed = 30.0F;
	private Quaternion rot;
	private float compareAngle, distance;

	private bool gyroEnabled;
	private Gyroscope gyro;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;

		gyroEnabled = EnableGyro();
	}

	private bool EnableGyro(){
		if(SystemInfo.supportsGyroscope)
		{
			gyro = Input.gyro;
			gyro.enabled = true;


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
		// directional up and down for moving up or down using mouse and gyro------DONE
		// do the camera adjust positon
		// the bird axis rotates
		// the widening of camera angle

		float bias = 0.96f;

		Vector3 moveCamTo = transform.position - transform.forward * 7.0f + transform.up * 5.0f;
		attachedCamera.transform.position = attachedCamera.transform.position * bias + moveCamTo * (1.0f - bias);
		attachedCamera.transform.LookAt (transform.position + transform.forward * 30.0f);
		//attachedCamera.transform.Rotate (birdModel.transform.rotation);

		//attachedCamera.transform.lookRotation (0.0f, 0.0f, Input.GetAxis ("Horizontal"));

		speed -= transform.forward.y;

		if (speed < 5.0f) {
			speed = 5.0f;
		}

		transform.position += transform.forward * Time.deltaTime * speed;
		transform.Rotate (Input.GetAxis("Vertical"), 0.0f, Input.GetAxis ("Horizontal"));
		//birdModel.transform.position = transform.position + spaceBuffer;

		//compare the angle/distance between mouse and point on path.
		//CompareAngle ();

		//Debug
		Debug.Log("DISTANCE: " + distance);
		Debug.Log("SPEED: " + speed);

		//float terrainHeightWhereWeAre = Terrain.activeTerrain.SampleHeight( transform.position );
		//if (terrainHeightWhereWeAre > transform.position.y){
		//	transform.position = new Vector3(transform.position.x,
		//									 terrainHeightWhereWeAre,
		//									 transform.position.z);
		//}
	}

	private void CompareAngle(){

		Vector3 worldPoint;
		Ray ray;// mouse Vector3 ray into real space 
		Quaternion directLook, playerLook;

		//get direction of point on path in relation to camera
		//		centerPoint = attachedCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

		//get the two orientation/directions
		directLook = Quaternion.LookRotation(birdModel.transform.position - attachedCamera.transform.position);


		//apply the mouseLook orienation to the camera
		if (gyroEnabled) {
			//			directLook *= rot;

			Quaternion referenceRotation = directLook;
			Quaternion deviceRotation = gyro.attitude;
			Quaternion eliminationOfXY = Quaternion.Inverse(
				Quaternion.FromToRotation(referenceRotation * Vector3.up, 
					deviceRotation * Vector3.up));
			Quaternion rotationZ = eliminationOfXY * deviceRotation;
			playerLook = rotationZ;

			//			playerLook = gyro.attitude *  directLook;//

		} else {
			//get ray of mouse to calculate real point in space
			ray = attachedCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue);// draw ray for now

			//get direction of point on path in relation to camera offset by mouse ray direction/point
			worldPoint = (birdModel.transform.position + ray.direction) - attachedCamera.transform.position;

			playerLook = Quaternion.LookRotation (worldPoint);

		}

		//this is the rotation.
		attachedCamera.transform.localRotation = playerLook;


		//Compare the Quaternions by getting their angle between two
		//		compareAngle = Quaternion.Angle (playerLook, directLook);




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
