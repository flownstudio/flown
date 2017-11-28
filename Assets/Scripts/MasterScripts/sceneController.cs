using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneController : MonoBehaviour {

	public Camera[] cameraList;
	private int currentCamera;

	public Swipe swipeControls;

	void Start () 
	{
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
		//Increase the camera index to get the next camera
		if (swipeControls.SwipeUp){
			SwitchPov ();
		}	
	}


	void SwitchPov () {
		currentCamera++;

		//Camera switcher
		if (currentCamera < cameraList.Length) {//Check if it's the last in the array...
			cameraList [currentCamera - 1].gameObject.SetActive (false);
			cameraList [currentCamera].gameObject.SetActive (true);
			Cursor.visible = true;
		}else {//if it is, current camera is the first in the array
			cameraList [currentCamera - 1].gameObject.SetActive (false);
			currentCamera = 0;
			cameraList [currentCamera].gameObject.SetActive (true);
			Cursor.visible = false;
		}


		//Bird Auto-pilot


	}
}
