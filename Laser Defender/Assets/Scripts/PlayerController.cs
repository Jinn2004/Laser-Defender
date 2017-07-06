using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float Xspeed = 3.0f;
	public float Yspeed = 3.0f;
	public float padding = 0.5f;
	public GameObject projectileOne;
	public GameObject projectileTwo;	
	public float projectileSpeed = 5f;
	public float firingRate = 0.2f;
	public float health = 250f;
	public bool changeFire = false;

	public AudioClip fireSound;

	// These will restrict the boundaries of the game.
	float xMin;
	float xMax;
	float yMin;
	float yMax;

	void Start(){
		// This is to get the cameras x, y, and z corrdinates, so we can restrict the plane to the camera size
		// no matter what the size is.
		float zDistance = transform.position.z - Camera.main.transform.position.z;
		//float yDistance = transform.position.y - Camera.main.transform.position.y;
		Vector3 leftMost   = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, zDistance));
		Vector3 rightMost  = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, zDistance));
		Vector3 topMost    = Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, zDistance));
		Vector3 bottomMost = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, zDistance));
		
		// For figuring out the top and bottom restrictions
		//Vector3 topMost    = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, zDistance));
		//Vector3 bottomMost = Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, zDistance));

		xMin = leftMost.x  + padding;
		xMax = rightMost.x - padding;

		yMin = bottomMost.y + padding;
		yMax = topMost.y    - padding;

		// The ymin and ymax restrictions numbers, This should be good.
		//
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			changeFire = true;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			changeFire = false;
		}

		// Invoke gives a good steady rate of fire.
		if (Input.GetKeyDown (KeyCode.Space)) {
			// The InvokeRepeating method is What function to call, then time, and speed)
			InvokeRepeating("Fire", 0.00001f, firingRate);
			//Fire();  This method is still called because of the string above.
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("Fire");
		}

		// Left and Right Controls
		if        (Input.GetKey (KeyCode.LeftArrow)   || Input.GetKey(KeyCode.A)) {
			transform.position += new Vector3 (-Xspeed * Time.deltaTime, 0, 0);
		} else if (Input.GetKey (KeyCode.RightArrow)  || Input.GetKey(KeyCode.D)) {
			transform.position += new Vector3 ( Xspeed * Time.deltaTime, 0, 0);
		}

		// Up and Down Controls
		if        (Input.GetKey (KeyCode.DownArrow)   || Input.GetKey(KeyCode.S)) {
			transform.position += new Vector3 (0, -Yspeed * Time.deltaTime, 0);
		} else if (Input.GetKey (KeyCode.UpArrow)     || Input.GetKey(KeyCode.W)) {
			transform.position += new Vector3 (0,  Yspeed * Time.deltaTime, 0);
		}

		// Another way to write it would be to use a line of code as follows
		// transform.position += Vector3.left * Xspeed + Time.deltaTime;

		// Restrict the player gamespace
		float NewX = Mathf.Clamp (transform.position.x, xMin, xMax);
		float NewY = Mathf.Clamp (transform.position.y, yMin, yMax);
		transform.position = new Vector3 (NewX, transform.position.y, transform.position.z);
		transform.position = new Vector3 (transform.position.x, NewY, transform.position.z);
		
		//transform.position = new Vector3 (NewX, NewY/*transform.position.y*/, transform.position.z);
		//transform.position = new Vector3 (transform.position.x, NewY, transform.position.z);
		
	}

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
				Die();
			}
		}
		
	}

	void Die(){
		Destroy (gameObject);
		LevelManager Man = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		Man.LoadLevel("Win");
	}

	void Fire(){
		if (changeFire == true) {
			// Laser Spawn
			float Dual = 0.4f;
			float split = 1f;
			Vector3 offSet = new Vector3 (Dual, 0f, 0f);
			GameObject beamZero = Instantiate (projectileOne, transform.position + offSet, Quaternion.identity) as GameObject;
			GameObject beamOne = Instantiate (projectileOne, transform.position - offSet, Quaternion.identity) as GameObject;
			beamZero.rigidbody2D.velocity = new Vector3 (split, projectileSpeed, 0f);
			beamOne.rigidbody2D.velocity = new Vector3 (-split, projectileSpeed, 0f);
			AudioSource.PlayClipAtPoint (fireSound, transform.position);
			firingRate = 0.6f;
		} else {
			GameObject beamZero = Instantiate (projectileTwo, transform.position, Quaternion.identity) as GameObject;
			beamZero.rigidbody2D.velocity = new Vector3 (0f, projectileSpeed, 0f);
			AudioSource.PlayClipAtPoint (fireSound, transform.position);
			firingRate = 0.15f;
			
		}
	}

}
