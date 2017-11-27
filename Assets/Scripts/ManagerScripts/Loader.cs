using UnityEngine;
using System.Collections;


public class Loader : MonoBehaviour 
{
	public GameObject gameManagerInstance;          //GameManager prefab to instantiate.
	public GameObject soundManagerInstance;         //SoundManager prefab to instantiate.     

	void Awake ()
	{

		//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
		if (gameManager.instance == null) {

			//Instantiate gameManager prefab
			Instantiate (gameManagerInstance);
		}

		//Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
//		if (soundManager.instance == null) {
//
//			//Instantiate SoundManager prefab
//			Instantiate (soundManagerInstance);
//		}
	}
}