using UnityEngine;
using System.Collections;

public class windBoost : MonoBehaviour
{
    public float thrust;
    public GameObject playerObject;

    void Start()
    {

    }

    void OnTriggerEnter(Collider other) {

        //playerObject.GetComponent<PlayerControllerMaster>().speed *= thrust;

    }

    void OnTriggerStay(){

        playerObject.GetComponent<PlayerControllerMaster>().speed += 3*thrust+Mathf.PerlinNoise(Time.deltaTime, 0.0F);

    }

}