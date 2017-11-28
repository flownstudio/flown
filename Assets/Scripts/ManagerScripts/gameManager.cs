using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;       //Allows us to use Lists. 

public class gameManager : MonoBehaviour
{

	public static gameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
	private worldManager worldScript;                       //Store a reference to our BoardManager which will set up the level.
	private int level = 0;                                  //Current level number, expressed in game as "Day 1".

	//Awake is always called before any Start functions
	void Awake()
	{
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

		//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
		Destroy(gameObject);    

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		//Instead of having one world manager for the whole game
		//Have scene prefabs with world managers and progressively more interesting/diffcult challenges
		//Have the weather/season change.
		//
		worldScript = GetComponent<worldManager>();

		Scene scene = SceneManager.GetActiveScene();
		level = scene.buildIndex;
		Debug.Log("Active scene is '" + scene.buildIndex + "'.");

		InitGame ();
	}

	//Initializes the game for each level.
	void InitGame()
	{
		//Call the SetupScene function of the BoardManager script, pass it current level number.

		if(level != 0){
			worldScript.SetupScene(level);
		}else {
			Cursor.visible = true;
		}

	}

	//Load Scene and destroy all objects besides Game Manager
	public static void LoadScene(int l)
	{
		Debug.Log ("Clicked");
		//		loadingImage.SetActive(true);
		SceneManager.LoadScene(l);
	}


	void OnLevelWasLoaded(int level){
		Debug.Log ("Level "+level+" was loaded.");

		if (level != 0) {
			
			worldScript.SetupScene (level);
		} else {
			Cursor.visible = true;
		}

	}


	//Update is called every frame.
	void Update()
	{

	}
}