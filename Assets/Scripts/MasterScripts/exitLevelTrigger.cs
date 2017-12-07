using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitLevelTrigger : MonoBehaviour {

	public int nextScene;

    void OnTriggerEnter(Collider other) {

       //call the game manager to next level?
		Debug.Log("HERE");
		gameManager.activateSceneChallenges (false);
		gameManager.LoadSceneAdditive(nextScene);

    }


}
