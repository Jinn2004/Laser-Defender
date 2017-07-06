using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	public float health = 150f;
	public float ProjectileSpeed = 5f;
	public float ShotsPerSec = 0.5f;
	public int scoreValue = 100;
	private ScoreKeeper scoreKeeper;
	public AudioClip fireSound;
	public AudioClip deathSound;
	

	void Start(){
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}

	void Update(){
		// give a random shot speed for enemies.
		float probability = Time.deltaTime * ShotsPerSec;
		if(Random.value < probability){
			Fire();
		}
	}

	void Fire(){
		Vector3 offSet = transform.position + new Vector3 (0.0f, 0f, 0.0f);
		GameObject missle = Instantiate (projectile, offSet, Quaternion.identity) as GameObject;
		missle.rigidbody2D.velocity = new Vector2 (0f, -ProjectileSpeed);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
		
	}

	public GameObject projectile;

	void OnTriggerEnter2D(Collider2D collider){
		// destroy what colides with this
		//Destroy (collider.gameObject);
		// will give 100 damage on collision
		Projectile missle = collider.gameObject.GetComponent<Projectile> ();
		if (missle) {
			health -= missle.GetDamage();
			missle.Hit();
			if(health <= 0)
			{
				Die();			}
		}
	}

	void Die(){
		AudioSource.PlayClipAtPoint(deathSound, transform.position);
		Destroy(gameObject);
		scoreKeeper.scorePoints(scoreValue);
	}	
}
