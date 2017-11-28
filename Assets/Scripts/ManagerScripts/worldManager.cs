using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class worldManager : MonoBehaviour {

	[Serializable]

	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}



	public int columns = 8;                                         //Number of columns in our game board.
	public int rows = 8;                                            //Number of rows in our game board.
	public Count windTunnelCount = new Count (5, 9);                      //Lower and upper limit for our random number of walls per level.

	//Just a big hoop you fly through to get to the next level/season?
	public GameObject exit;                                         //Prefab to spawn for exit.
	public GameObject[] windTunnels;                                 //Array of floor prefabs.


	private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
	private List <Vector3> gridPositions = new List <Vector3> ();   //A list of possible locations to place tiles.


	//I think this might be a good method to think in terms of a grid with challenges laid out
	//would need to change the "size" of the grids
	void InitialiseList ()
	{
		//Clear our list gridPositions.
		gridPositions.Clear ();

		//Loop through x axis (columns).
		for(int x = 1; x < columns-1; x++)
		{
			//Within each column, loop through y axis (rows).
			for(int y = 1; y < rows-1; y++)
			{
				//At each index add a new Vector3 to our list with the x and y coordinates of that position.
				gridPositions.Add (new Vector3(x, y, 0f));
			}
		}
	}


	//Do we need this??
	void TerrainSetup ()
	{
		GameObject.Find("mapGenerator").GetComponent<endlessTerrain>().initTerrain();

	}


	//RandomPosition returns a random position from our list gridPositions.
	Vector3 RandomPosition ()
	{
		//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
		int randomIndex = Random.Range (0, gridPositions.Count);

		//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
		Vector3 randomPosition = gridPositions[randomIndex];

		//Remove the entry at randomIndex from the list so that it can't be re-used.
		gridPositions.RemoveAt (randomIndex);

		//Return the randomly selected Vector3 position.
		return randomPosition;
	}


	//LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
	void LayoutObjectAtRandom (GameObject[] prefabArray, int minimum, int maximum)
	{
		//Choose a random number of objects to instantiate within the minimum and maximum limits
		int objectCount = Random.Range (minimum, maximum+1);

		//Instantiate objects until the randomly chosen limit objectCount is reached
		for(int i = 0; i < objectCount; i++)
		{
			//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
			Vector3 randomPosition = RandomPosition();

			//Choose a random tile from tileArray and assign it to tileChoice
			GameObject prefabChoice = prefabArray[Random.Range (0, prefabArray.Length)];

			//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
			Instantiate(prefabChoice, randomPosition, Quaternion.identity);
		}
	}


	//SetupScene initializes our level and calls the previous functions to lay out the game board
	public void SetupScene (int level)
	{
		Debug.Log ("Do Something CRazy");
			TerrainSetup ();

			//Reset our list of gridpositions.
//			InitialiseList ();

			//Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
//			LayoutObjectAtRandom (windTunnels, windTunnelCount.minimum, windTunnelCount.maximum);

			//NICE bit of math using math.log to scale up difficulty as levels progress
			//Determine number of enemies based on current level number, based on a logarithmic progression
			//		int enemyCount = (int)Mathf.Log(level, 2f);

			//Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
			//		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);

			//Instantiate the exit tile in the upper right hand corner of our game board
//			Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
				

	}

}
