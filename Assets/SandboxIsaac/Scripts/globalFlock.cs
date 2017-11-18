using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalFlock : MonoBehaviour {


	public GameObject birdObject;
	public GameObject goalPrefab;
	public static int sceneSize = 100;

	public int playerSuccess = 1;
	Vector3 playerPosition;

	static int numOfBirds = 100;
	public static GameObject[] allBirds = new GameObject[numOfBirds];
	public static Vector3 headingPos = Vector3.zero;

	public bool switchDirectionX = false;
	public bool switchDirectionY = false;
	public bool switchDirectionZ = false;
	public float zPos = 0;
	public float xPos = 0;
	public float yPos = 6;

	// Use this for initialization
	void Start () {
		
		for(int i = 0; i < numOfBirds; i++){
			//Generate a position around current player position
			Vector3 pos = new Vector3(
						Random.Range(-sceneSize/20, sceneSize/20),
						Random.Range(5, sceneSize/20),
						Random.Range(-sceneSize/20, sceneSize/20));

			//add bird to all birds 
			allBirds[i] = (GameObject) Instantiate(birdObject, pos, Quaternion.identity);
			// TODO : make bird invisible first
			birdObject.SetActive(false);

		}
		
	}
	
	// Update is called once per frame
	void Update () {


		headingPos = new Vector3(xPos, yPos, zPos);
		goalPrefab.transform.position = headingPos;

		xPos += Random.Range(-0.2f,0.2f);
		

		yPos += Random.Range(-0.2f,0.2f);
		if(yPos <= 5){
			yPos += Random.Range(0.2f,0.4f);
		}

		if(zPos >= sceneSize ||  zPos <= -sceneSize){
			switchDirectionZ = !switchDirectionZ;
		}
		if(!switchDirectionZ){
			zPos += 0.05f;
		}else{
			zPos -= 0.05f;
		}


		playerPosition = GameObject.Find("player").transform.position;
		playerSuccess = GameObject.Find("player").GetComponent<playerScriptIsaac>().successRating;
		if(playerSuccess > 100){
			playerSuccess = 100;
		}
		birdVisibility();
		
	}

	void birdVisibility(){

		GameObject[] gos;
		gos = globalFlock.allBirds;
		//switch birds on up to count
		for(int i = 0; i < playerSuccess; i++){	
			if(!gos[i].active){
				gos[i].transform.position = new Vector3(
						UnityEngine.Random.Range(playerPosition.x-1, playerPosition.x),
						UnityEngine.Random.Range(playerPosition.y-1, playerPosition.y),
						UnityEngine.Random.Range(playerPosition.z-2, playerPosition.z-1));
				gos[i].SetActive(true);
				// TODO : fade in bird
			}

		}
		//switch birds off
		for(int i = playerSuccess; i < numOfBirds; i++){
			if(gos[i].active){
				// TODO : fade out bird then after time setactive false.
				gos[i].SetActive(false);
			}
		}

		
	}


}
