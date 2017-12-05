using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneController : MonoBehaviour {

	public Camera[] cameraList;
	public GameObject player;
	private int currentCamera;

	public Swipe swipeControls;


	public float successRating = 10;
	public float speed = 12f;
	public float distanceFromFlock;
	public float distanceFromGround;
	public float dangerDistance = 12f;
	public float playerSpeed;

	public bool autoPilot = false;

	bool turning = false;
	bool scoreDirectionUp = true;
	//TODO: get max num of birds from global flock
	int maxNumOfBirds = 200;

	private PlayerControllerMaster playerControllerMaster;
	private Material playerMaterial;



	void Start () 
	{
		playerControllerMaster = player.GetComponent<PlayerControllerMaster> ();
		playerSpeed = playerControllerMaster.speed;

		Renderer renderer = player.transform.Find("rotated_starling_UV").gameObject.GetComponent<Renderer>();
		playerMaterial = renderer.sharedMaterials[0];

		//Disable all cameras and make the first in the list enabled

		currentCamera = 0;

		for (int i = 0; i < cameraList.Length; i++)
		{
			cameraList [i].gameObject.SetActive(false);
		}

		if (cameraList.Length > 0)
		{
			cameraList [0].gameObject.SetActive(true);
		}
			

	}
	

	void Update () 
	{
		playerStats ();
		//Increase the camera index to get the next camera
		if (swipeControls.SwipeUp){
			SwitchPov ();
		}	
	}


	void SwitchPov () {
		currentCamera++;

		//Camera switcher
		if (currentCamera < cameraList.Length) {//Check if it's the last in the array... WIDE ANGLE
			cameraList [currentCamera - 1].gameObject.SetActive (false);
			cameraList [currentCamera].gameObject.SetActive (true);
			Cursor.visible = true;
			autoPilot = true;
			playerMaterial.DisableKeyword("_EMISSION");
		}else {//if it is, current camera is the first in the array BIRD VIEW
			cameraList [currentCamera - 1].gameObject.SetActive (false);
			currentCamera = 0;
			cameraList [currentCamera].gameObject.SetActive (true);
			Cursor.visible = false;
			autoPilot = false;
			playerMaterial.EnableKeyword("_EMISSION");
		}


	}

	void playerStats() {
		// This is just to test the visibility of other birds adding them in
		// one at a time.
		// 1% chance this frame adds a bird up to max birds then goes back down

		if (!autoPilot) {// hack for now so birds don't pop in while wide angle POV is on
			
			if (UnityEngine.Random.Range (0, 100) < 1) {
				if (scoreDirectionUp) {
					successRating += 1;
					if (successRating == maxNumOfBirds) {
						scoreDirectionUp = false;
					}
				} else {
					successRating -= 1;
					if (successRating == 0) {
						scoreDirectionUp = true;
					}
				}
			}
		}
		// TODO: adjust flock speed depending on distance
		distanceFromFlock = Vector3.Distance(globalFlock.headingPos, this.transform.position);
		Vector3 groundPoint = new Vector3(this.transform.position.x, 0, this.transform.position.z);
		distanceFromGround = Vector3.Distance(this.transform.position, groundPoint);

		//move forward
		//transform.Translate(0,0, Time.deltaTime * speed);
	}
}
