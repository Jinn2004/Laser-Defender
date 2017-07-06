using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float damage = 100f;

	public void Hit(){
		// Destroy the laser on hit.
		Destroy (gameObject);
	}

	public float GetDamage(){
		return damage;
	}
}
