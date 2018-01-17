using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;       //Allows us to use Lists. 

public class gameManager : MonoBehaviour
{

	public static gameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.                     //Store a reference to our BoardManager which will set up the level.
	public static int level = 0;                                  //Current level number, expressed in game as "Day 1".
	public static GameObject challenges = null;

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

		Scene scene = SceneManager.GetActiveScene();
		level = scene.buildIndex;
		Debug.Log("Active scene is '" + scene.buildIndex + "'.");

//		getSceneChallenges ();

		InitGame ();
	}

	//Initializes the game for each level.
	void InitGame()
	{

		if(level != 0){//This is just here so we can start game from main not have to use menu scene always
			GameObject.Find("mapGenerator").GetComponent<endlessTerrain>().initTerrain ();
			GameObject[] rootObjects = SceneManager.GetSceneByBuildIndex (level).GetRootGameObjects();

			foreach (GameObject obj in rootObjects) {
				if (obj.name == "SceneChallenges")
					challenges = obj;
			}

//			Cursor.visible = false;
		}else {
//			Cursor.visible = true;
		}

	}

	public static void activateSceneChallenges(bool activate){
		
		if (level != 0) {
			if (activate && challenges) { //setting the challegnes from inactive to active
				challenges.SetActive (true);
			} else{// finding the active scene challenges, storing it first then set to inactive
				challenges.SetActive (false);
			} 
		} 

	}

	//Load Scene and destroy all objects besides Game Manager
	public static void LoadScene(int l)
	{
		Debug.Log ("Clicked");
		//		loadingImage.SetActive(true);
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
		SceneManager.LoadScene(l);
	}

	public static void LoadSceneAdditive(int l)
	{
		Debug.Log ("Clicked");
		//		loadingImage.SetActive(true);
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
		SceneManager.LoadScene(l, LoadSceneMode.Additive);

	}

	public static void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
		Debug.Log ("Level "+scene+" was loaded. Mode: "+mode);
		level = scene.buildIndex;

		Debug.Log (level);

		if (level != 0) {
			if (level == 1) {
				GameObject.Find ("mapGenerator").GetComponent<endlessTerrain> ().initTerrain ();
//				Cursor.visible = false;
			}

			GameObject[] rootObjects = SceneManager.GetSceneByBuildIndex (level).GetRootGameObjects();

			foreach (GameObject obj in rootObjects) {
				if (obj.name == "SceneChallenges")
					challenges = obj;
			}

//			Cursor.visible = false;
			SceneManager.sceneLoaded -= OnLevelFinishedLoading;
		}else{
//			Cursor.visible = true;
			SceneManager.sceneLoaded -= OnLevelFinishedLoading;
		}

	}

	//Update is called every frame.
	void Update()
	{

	}
}