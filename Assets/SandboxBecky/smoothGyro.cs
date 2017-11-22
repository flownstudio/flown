using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoothGyro : MonoBehaviour {
	//Public variables
	public Swipe swipeControls;

	private float speed = 8.0F;
	private Quaternion rot;
	private float distance, eulerZ, eulerX, eulerY;
	private Vector3 velocity;
	private Rigidbody rigidbody;

	private bool gyroEnabled;
	private Gyroscope gyro;


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



	// Use this for initialization
	void Start () {
		Cursor.visible = false;

		gyroEnabled = EnableGyro();
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

		RotatePlayer ();

	}

	private void FixedUpdate(){

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
		velocity = rigidbody.velocity;
		//		Debug.Log (rigidbody.velocity);
		Debug.Log(transform.forward + " " + Vector3.forward);

		//		localVelocity = transform.InverseTransformDirection(rigidbody.velocity);  
	}

	//Gyro and/or look around
	private void RotatePlayer(){

		//apply the mouseLook orienation to the camera
		if (gyroEnabled) {
			//			directLook *= rot;

			//			Quaternion referenceRotation = directLook;
			//			Quaternion deviceRotation = gyro.attitude;
			//			Quaternion eliminationOfZX = Quaternion.Inverse(
			//				Quaternion.FromToRotation(referenceRotation * Vector3.up, 
			//					deviceRotation * Vector3.up)
			//			);
			//			Quaternion rotationY = eliminationOfZX * deviceRotation;
			//			playerLook = rotationY;



			//			eulerX = playerLook.eulerAngles.x;
			//			eulerY = playerLook.eulerAngles.y;
			//			eulerZ = playerLook.eulerAngles.z;

			//			playerLook = gyro.attitude *  directLook;//
			Quaternion test = Quaternion.Slerp(transform.rotation,
				cameraBase * ( ConvertRotation(referanceRotation * Input.gyro.attitude) ), lowPassFilterFactor);
			transform.rotation = test;

			eulerX = test.eulerAngles.x;
			eulerY = test.eulerAngles.y;
			eulerZ = test.eulerAngles.z;

		} 




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



	void OnGUI() {
		//		string gui = "X: " + eulerX + ", Y: " + eulerY +", Z: " + eulerZ;
		string gui = "Speed: " + speed;
		GUI.Label(new Rect(10, 10, 500, 20), gui );
	}





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



