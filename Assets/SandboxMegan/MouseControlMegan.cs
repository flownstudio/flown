using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControlMegan : MonoBehaviour {
	//Public variables
	public GameObject target;//This is the invisible target point on the path or whatever
	public GameObject birdPlayer;

	private float speed = 1.0F;
	private GameObject cameraContainer;
	private Quaternion rot;
	private float compareAngle, distance;
	private float birdSpeed = 90.0f;

	// Use this for initialization
	void Start () {
		cameraContainer = new GameObject ("Camera Container");
		Cursor.visible = false;

		//transforms cameraContainer to camera position
		cameraContainer.transform.position = transform.position;
		//Sets cameraConainer as parent of camera
		transform.SetParent (cameraContainer.transform);

	}

	private void Update(){

		transform.position += transform.forward * Time.deltaTime * birdSpeed;
		transform.Rotate (Input.GetAxis ("Vertical"), 0.0f, Input.GetAxis ("Horizontal"));

	}

	private void CompareAngle(){

		//BECKY
		//Give directional look angle too...
		// create direction too 


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
		worldPoint = (target.transform.position + (ray.direction*3)) - transform.position;

		//get the two orientation/directions
		directLook = Quaternion.LookRotation(relativePos);
		mouseLook = Quaternion.LookRotation (worldPoint);	

		//apply the mouseLook orienation to the camera
		transform.localRotation = mouseLook;

		//Compare the Quaternions by getting their angle between two
		compareAngle = Quaternion.Angle (mouseLook, directLook);

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
