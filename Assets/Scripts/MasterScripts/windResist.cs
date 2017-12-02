using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windResist : MonoBehaviour {

    public GameObject playerObject;


    void OnTriggerEnter(Collider other) {

       
    }

    void OnTriggerStay(Collider other){

    	 
    	playerObject.GetComponent<PlayerControllerMaster>().speed -= Mathf.PerlinNoise(Time.deltaTime, 0.0F);


    }

}
