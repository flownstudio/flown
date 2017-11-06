using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour {

public Transform target;
public float smoothSpeed;
public Vector3 offset;

	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (target)
		{
			Vector3 desiredPosition = target.position + offset;
			Vector3 smoothedPosition = Vector3.Lerp (transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
			Vector3 worldUp = Vector3.up;
			transform.position = smoothedPosition; 
			//transform.rotation = Quaternion.Slerp (transform.rotation, target.rotation, Time.deltaTime * 1);
			transform.LookAt(target, worldUp);
		}
	}
}
