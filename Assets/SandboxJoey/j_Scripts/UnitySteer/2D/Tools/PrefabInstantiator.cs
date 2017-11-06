using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabInstantiator : MonoBehaviour {

	[SerializeField]
	GameObject _prefab;

	[SerializeField]
	int _amount = 100;

	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < _amount; i++)
		{
			Instantiate(_prefab, transform.position, Quaternion.identity);
		}
		Destroy(this);
			
	}
	

}
