using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalFlock : MonoBehaviour {


	public GameObject birdObject;
	public GameObject goalPrefab;
	public static int sceneSize = 100;
	static int numOfBirds = 150;
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
		for(int i = 0; i < numOfBirds; i++)
		{
			Vector3 pos = new Vector3(
						Random.Range(-sceneSize/20, sceneSize/20),
						Random.Range(5, sceneSize/20),
						Random.Range(-sceneSize/20, sceneSize/20));
			allBirds[i] = (GameObject) Instantiate(birdObject, pos, Quaternion.identity);
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
	
		
	}
}
