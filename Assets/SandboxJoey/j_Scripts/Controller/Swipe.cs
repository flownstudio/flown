using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
	private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
	private bool isDragging;
	public bool isHolding;

	private Vector2 startTouch, swipeDelta;

	private void Update()
	{
		//make them false at the first action of the frame, then once we stop inputting, they return to false
		tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

		//DESKTOP / STANDALONE INPUTS

		if (Input.GetMouseButtonDown (0))
		{
			isDragging = true;
			isHolding = true;
			tap = false;
			Debug.Log ("Holding..");
			startTouch = Input.mousePosition;
		}

		//On release, reset...
		else if (Input.GetMouseButtonUp(0))
		{
			tap = true;
			isDragging = false;
			isHolding = false;
			Debug.Log ("Tap..");
			Reset();
		}

		//MOBILE INPUTS

		if (Input.touches.Length > 0) //if there is 1 finger touching or more...
		{
			if (Input.touches[0].phase == TouchPhase.Began || Input.touches[0].phase == TouchPhase.Stationary)
			{
				//key down behaviour
				isDragging = true;
				isHolding = true;
				Debug.Log ("Holding..");
				//Debug.Log ("Dragging..");
				startTouch = Input.touches[0].position;
			}


			//On release, reset...  Have to use ended OR cancelled for touch input
			else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
			{
				//key up behaviour
				Debug.Log ("Released..");
				tap = true;
				Reset();
			}
		}

		//Calculate the distance
		swipeDelta = Vector2.zero;

		if (isDragging)
		{

			if (Input.touches.Length > 0) {
				swipeDelta = Input.touches[0].position - startTouch;
				isHolding = true;
				Debug.Log ("Holding..");
			}
			else if (Input.GetMouseButton(0))
			{
				swipeDelta = (Vector2)Input.mousePosition - startTouch;
			}
			// else if (Input.touches.Length > 0.5 && Input.GetMouseButton(0)){
      //
			// }
		}

		//Did we cross the deadzone?
		if (swipeDelta.magnitude > 100)
		{
			//Which direction is the swipe?
			float x = swipeDelta.x;
			float y = swipeDelta.y;

			if (Mathf.Abs (x) > Mathf.Abs (y))
			{
				//Left or right
				if (x < 0)
				{
					Debug.Log ("Swipe left..");
					swipeLeft = true;
				}
				else
				{
					Debug.Log ("Swipe right..");
					swipeRight = true;
				}
			}

			else
			{
				//Up or down
				if (y < 0)
				{
					Debug.Log ("Swipe down..");
					swipeDown = true;
				}
				else
				{
					Debug.Log ("Swipe up..");
					swipeUp = true;
				}
			}

			Reset();
		}
	}



	private void Reset()
	{
		//reset to zero
		startTouch = swipeDelta = Vector2.zero;
		isDragging = false;
		isHolding = false;
		// Debug.Log ("not holding...");
	}


	//make variables public and available to other scripts (note capitalisation!)
	public bool Tap {get {return tap; } }
	// public bool isHolding {get {return holding; } }
	public Vector2 SwipeDelta { get {return swipeDelta; } }
	public bool SwipeLeft {get {return swipeLeft; } }
	public bool SwipeRight {get {return swipeRight; } }
	public bool SwipeUp {get {return swipeUp; } }
	public bool SwipeDown {get {return swipeDown; } }





}
