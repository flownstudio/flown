using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windThermalUp : MonoBehaviour {


    public GameObject playerObject;


    void OnTriggerEnter(Collider other) {

       
    }

    void OnTriggerStay(Collider other){

    	// playerObject.transform.Translate(Vector3.up * Time.deltaTime * 100 * (2 * Mathf.PerlinNoise(Time.deltaTime, 0.0F)) );
   
    	playerObject.transform.Translate(Vector3.up * Time.deltaTime * 100);


    }


}
