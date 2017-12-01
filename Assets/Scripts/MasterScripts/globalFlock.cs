using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalFlock : MonoBehaviour {

	public GameObject sceneController;
	public GameObject player;
	public GameObject birdObject;
	public GameObject goalPrefab;
	public static int sceneSize = 100;

	public float playerSuccess = 1;
	Vector3 playerPosition;

	public static int numOfBirds = 300;
	public static GameObject[] allBirds = new GameObject[numOfBirds];
	public static Vector3 headingPos = Vector3.zero;

	public bool switchDirectionX = false;
	public bool switchDirectionY = false;
	public bool switchDirectionZ = false;
	public float zPos = 0;
	public float xPos = 0;
	public float yPos = 12;

	public float headingMaxDistanceFromPlayer = 80;
	public bool waitingForPlayer = false;

	private sceneController playerStats;

	// Use this for initialization
	void Start () {
		playerStats = sceneController.GetComponent<sceneController> ();
		// make the birds up to max num of birds
		for(int i = 0; i < numOfBirds; i++){
			//Generate a position around current player position
			Vector3 pos = new Vector3(0,0,0);
			// add bird to list of birds
			allBirds[i] = (GameObject) Instantiate(birdObject, pos, Quaternion.identity);
			

			birdObject.SetActive(false);

			// TODO : starting color of fade in bird, something like this:
            //Color color = new Color(1, 1, 1, 0);
            //birdObject.transform.GetComponent<Renderer>().material.SetColor("_Color", color);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// update visibilit of birds in flock based upon player score.
		playerSuccess = sceneController.GetComponent<sceneController>().successRating;
		birdVisibility();

		// how far away from, the heading is the player
		playerPosition = player.transform.position;
		float distanceFromPlayer = Vector3.Distance(playerPosition, headingPos);
		// If the player is close enough to the heading point of the flock then update
		// the heading. Otherwise stay still till player catches up.
		if(UnityEngine.Random.Range(0,20) < 1){
			if(waitingForPlayer){
				if(distanceFromPlayer < 16){
					waitingForPlayer = false;
				}
			}else{
				if(distanceFromPlayer < headingMaxDistanceFromPlayer){
					headingPos = new Vector3(xPos, yPos, zPos);
					goalPrefab.transform.position = headingPos;
					xPos += Random.Range(-1f,1f);
					yPos += Random.Range(-1f,1f);

					//keep above ground
					if(yPos <= 11){
						yPos += Random.Range(1f,4f);
					}

					// if(zPos >= sceneSize ||  zPos <= -sceneSize){
					// 	switchDirectionZ = !switchDirectionZ;
					// }

					float speed = playerStats.speed;
					// TODO: zPos changes with player speed
					zPos += 5f;


				}else{
					waitingForPlayer = true;
				}
			}
			
		}
		
	}

	void birdVisibility(){

		GameObject[] gos;
		gos = globalFlock.allBirds;
		//switch birds on up to count
		for(int i = 0; i < playerSuccess; i++){	
			// if bird isnt active make active
			if(!gos[i].active){

				//TODO: TWEAK DISTANCE BEHIND
				// birds load in behind the player, rather than on player.z - distance. 

				float distanceBehind = 20;
				Vector3 backwardsFromPlayer = player.transform.position - (player.transform.forward * distanceBehind);
				gos[i].transform.position = backwardsFromPlayer;
 
				gos[i].SetActive(true);
				//FADE INfromAlpha(gos[i]);
			}
		}
		//switch birds off
		for(int i = (int)playerSuccess; i < numOfBirds; i++){
			if(gos[i].active){
				//FADE OUT ToAlpha(gos[i]);
				gos[i].SetActive(false);
			}
		}

		
	}

	//FADING IN SOMTHING LIKE THIS
	// void ToAlpha (GameObject birdObject) {
  
 //         float alpha = birdObject .material.color.a;
         
 //         while(alpha > 0) {
         
 //             alpha -= Time.deltaTime;
 //             Color newColor = new Color(1, 1, 1, alpha);
 //             birdObject. .material.color = newColor;
             
 //         }
         
 //     }
     
 //    void fromAlpha (GameObject birdObject) {
         
 //         float alpha = birdObject. .material.color.a;
         
 //             while(alpha < 1) {
             
 //             alpha += Time.deltaTime;
 //             Color newColor = new Color(1, 1, 1, alpha);
 //             birdObject. .material.color = newColor;
             
 //         }
         
 //    }

}
