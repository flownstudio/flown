using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroControl : MonoBehaviour {
	//Publib variables
	public GameObject target;
	public float speed;

	private bool gyroEnabled;
	private Gyroscope gyro;

	private GameObject cameraContainer;
	private Quaternion rot;

	//GUI string do mobile Debug
	private string eulerVals;

	// Use this for initialization
	void Start () {
		cameraContainer = new GameObject ("Camera Container");

		//transforms cameraContainer to camera position
		cameraContainer.transform.position = transform.position;
		//Sets cameraConainer as parent of camera
		transform.SetParent (cameraContainer.transform);

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
		if (gyroEnabled) {
			float compareAngle; 
			float step = speed * Time.deltaTime;

			//Compare the Quaternions... i.e. if Gyro is in LookRotation range Feeback == GOOD
			Vector3 relativePos = target.transform.position - transform.position;
			Quaternion rotation = Quaternion.LookRotation(relativePos);
			//		//Don't transform
//					transform.localRotation = rotation;

			transform.localRotation = gyro.attitude ;

			Debug.Log ("localRotation: " + transform.localRotation);
			Debug.Log ("Rotation: " + transform.rotation);
			Debug.Log ("LookRotation: " + rotation);

			compareAngle = Quaternion.Angle(transform.localRotation, rotation);

			if (compareAngle < 100) {
				speed = 3;
			} else {
				speed = 1;
			}

			//Move The Camera towards the target
			transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);


//			eulerVals = "X: " + transform.localEulerAngles.x + "\n Y: " + transform.localEulerAngles.y + "\n Z: " + transform.localEulerAngles.z;
			eulerVals = "ANGLE: " + compareAngle;

		}
			
	}
		
	void OnGUI() {
		GUI.Label(new Rect(10, 10, 500, 20), eulerVals);
	}
}
