using UnityEngine;
using System.Collections;

public class Shredder : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col){
		// THis will destroy anything that collides with the shredder
		Destroy (col.gameObject);
	}
}
