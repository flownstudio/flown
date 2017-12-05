using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMaster : MonoBehaviour {

	//Public variables
	public Swipe swipeControls;
	public GameObject sceneController;

	public Camera attachedCamera;// Model that we are putting in front of camera
//	public GameObject birdModel;
//	public Vector3 spaceBuffer;

	private Rigidbody rigidbody;
	public float speed = 30.0F;
	private float distance, eulerZ, eulerX, eulerY;

	private bool gyroEnabled;
	private Gyroscope gyro;
	private sceneController playerStats;


	//-------------------------------
	//Gyro calibration variables
	private const float lowPassFilterFactor = 0.2f;

	private readonly Quaternion baseIdentity =  Quaternion.Euler(90, 0, 0);
	//	private readonly Quaternion landscapeRight =  Quaternion.Euler(0, 0, 90);
	//	private readonly Quaternion landscapeLeft =  Quaternion.Euler(0, 0, -90);
	//	private readonly Quaternion upsideDown =  Quaternion.Euler(0, 0, 180);

	private Quaternion cameraBase =  Quaternion.identity;
	private Quaternion calibration =  Quaternion.identity;
	private Quaternion baseOrientation =  Quaternion.Euler(90, 0, 0);
	private Quaternion baseOrientationRotationFix =  Quaternion.identity;

	private Quaternion referanceRotation = Quaternion.identity;
	//--------------------------------

	// Use this for initialization
	void Start () {
		gyroEnabled = EnableGyro();
		playerStats = sceneController.GetComponent<sceneController> ();
		rigidbody = transform.GetComponent<Rigidbody>();
	}

	private bool EnableGyro(){
		if(SystemInfo.supportsGyroscope)
		{
			gyro = Input.gyro;
			gyro.enabled = true;

			ResetBaseOrientation();
			UpdateCalibration(true);
			UpdateCameraBaseRotation(true);
			RecalculateReferenceRotation();

			return true;
		}
		return false;
	}

	private void Update(){


		// Now you have the player stats available
		// playerStats.autoPilot
		// etc (check sceneController)
<<<<<<< HEAD


		if (gyroEnabled && !playerStats.autoPilot) {
			GyroPlayer ();
=======
>>>>>>> parent of a031ee9... Updated player controller


<<<<<<< HEAD
			Vector3 moveCamTo = transform.position - transform.forward * 7.0f + transform.up * 5.0f; // camera offset positions
			attachedCamera.transform.position = attachedCamera.transform.position * bias + moveCamTo * (1.0f - bias); // moves the camera, elastically and bias
			attachedCamera.transform.LookAt (transform.position + transform.forward * 30.0f); // rotating the camera

		} else if (!playerStats.autoPilot) {
=======
		if(gyroEnabled){
			GyroPlayer ();
		}else{
>>>>>>> parent of a031ee9... Updated player controller

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
			transform.Rotate (Input.GetAxis("Vertical"), 0.0f, Input.GetAxis ("Horizontal"));//HOW TO TRANSLATE DIRECTION TO MOBILE?? TIN!!!!
			//birdModel.transform.position = transform.position + spaceBuffer;



			//Debug
<<<<<<< HEAD
//			Debug.Log("DISTANCE: " + distance);
//			Debug.Log("SPEED: " + speed);
=======
			Debug.Log("DISTANCE: " + distance);
			Debug.Log("SPEED: " + speed);
>>>>>>> parent of a031ee9... Updated player controller

			//float terrainHeightWhereWeAre = Terrain.activeTerrain.SampleHeight( transform.position );
			//if (terrainHeightWhereWeAre > transform.position.y){
			//	transform.position = new Vector3(transform.position.x,
			//									 terrainHeightWhereWeAre,
			//									 transform.position.z);
			//}
		} else {
			transform.position += transform.forward * Time.deltaTime * speed;
		}



	}


	//This is for physics updates
	private void FixedUpdate(){


		if(gyroEnabled){
			TransformPlayer ();

			if (swipeControls.SwipeLeft) {
				//			if(speed < 0)speed *= -1;
				//			rigidbody.AddRelativeForce (transform.forward * speed,ForceMode.Acceleration);
				speed += 2.0F;
			}

			if (swipeControls.SwipeRight && speed > 1.0F) {
				//			if(speed > 0)speed *= -1;
				//			rigidbody.AddRelativeForce (transform.forward * speed,ForceMode.Acceleration);

				speed -= 1.0F;
			}

		}else{
			//Move megans stuff in here too 
		}
			



	}

	//For Physics
	private void TransformPlayer(){

		//how to add a little exaggerated sideways skid?
		//		rigidBody.position += transform.forward * Time.deltaTime * speed;
		//		birdModel.transform.position = transform.position + spaceBuffer;

		//		rigidBody.position += transform.forward * Time.deltaTime * speed;

		rigidbody.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);

		//		for when looking down AND then subtract when looking up
		//		rigidbody.AddRelativeForce (transform.forward * speed,ForceMode.Acceleration);
		//		rigidbody.velocity = transform.forward * Time.deltaTime * speed;

//		Debug.Log(transform.forward + " " + Vector3.forward);

		//		localVelocity = transform.InverseTransformDirection(rigidbody.velocity);  
	}

	//Gyro and/or look around
	private void GyroPlayer(){
		
		Quaternion test = Quaternion.Slerp(transform.rotation,
			cameraBase * ( ConvertRotation(referanceRotation * Input.gyro.attitude) ), lowPassFilterFactor);
		transform.rotation = test;

		eulerX = test.eulerAngles.x;
		eulerY = test.eulerAngles.y;
		eulerZ = test.eulerAngles.z;




		//`maybe to figure out direction

		//		if (eulerY > 180) {
		////			attachedCamera.transform 
		//		} else if (eulerY < 180) {
		//
		//		}
		//
		//		if (eulerX < 180 && speed <= 30.0) {
		//			speed += 2.0F;
		//		} else if (eulerX > 180 && speed >= 20.0){
		//			speed -= 1.0F;
		//		}

	
	}


	//TODO: make autopilot.... 
	// Now you have the player stats available
	// playerStats.autoPilot


	void OnGUI() {
		//		string gui = "X: " + eulerX + ", Y: " + eulerY +", Z: " + eulerZ;
		string gui = "Speed: " + speed;
		GUI.Label(new Rect(10, 10, 500, 20), gui );
	}






	//-------------------------------
	//`Gyro calibration functions

	/// <summary>
	/// Update the gyro calibration.
	/// </summary>
	private void UpdateCalibration(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = (Input.gyro.attitude) * (-Vector3.forward);
			fw.z = 0;
			if (fw == Vector3.zero)
			{
				calibration = Quaternion.identity;
			}
			else
			{
				calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
			}
		}
		else
		{
			calibration = Input.gyro.attitude;
		}
	}

	/// <summary>
	/// Update the camera base rotation.
	/// </summary>
	/// <param name='onlyHorizontal'>
	/// Only y rotation.
	/// </param>
	private void UpdateCameraBaseRotation(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = transform.forward;
			fw.y = 0;
			if (fw == Vector3.zero)
			{
				cameraBase = Quaternion.identity;
			}
			else
			{
				cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
			}
		}
		else
		{
			cameraBase = transform.rotation;
		}
	}

	/// <summary>
	/// Converts the rotation from right handed to left handed.
	/// </summary>
	/// <returns>
	/// The result rotation.
	/// </returns>
	/// <param name='q'>
	/// The rotation to convert.
	/// </param>
	private static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);	
	}


	/// <summary>
	/// Recalculates reference system.
	/// </summary>
	private void ResetBaseOrientation()
	{
		baseOrientation = Quaternion.identity * baseIdentity;
	}

	/// <summary>
	/// Recalculates reference rotation.
	/// </summary>
	private void RecalculateReferenceRotation()
	{
		referanceRotation = Quaternion.Inverse(baseOrientation)*Quaternion.Inverse(calibration);
	}

}
