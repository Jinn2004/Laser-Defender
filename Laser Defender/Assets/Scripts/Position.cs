using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	// This function will draw a sphere around an object so I can develope and see where I am placing,
	// It does not show on the screen when in game mode but helps to allow me to know a position.
	void OnDrawGizmos(){
		Gizmos.DrawWireSphere (transform.position, 1);	
		 
	}

}
