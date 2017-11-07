using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyMoveTarget : MonoBehaviour {
	private Vector3 startMarker;
	public float speed = 3.0F;
	private float startTime;
	private float journeyLength;
	public GameObject marker;


	void Start() {
		startTime = Time.time;
		startMarker = transform.position;

		pickRandomPoint();

	}

	void Update() {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;

//		Debug.Log (fracJourney);
		if (fracJourney >= 1.0) {
			startTime = Time.time;
			startMarker = marker.transform.position;
			pickRandomPoint ();

		}
		transform.position = Vector3.Lerp(startMarker, marker.transform.position, fracJourney);

	}

	void pickRandomPoint() {
		Vector3 newPoint = new Vector3 (Random.Range (-30.0f, 30.0f), Random.Range (-30.0f, 30.0f), Random.Range (-30.0f, 30.0f));
		marker.transform.position = newPoint;
		journeyLength = Vector3.Distance(startMarker, marker.transform.position);
	}
}
