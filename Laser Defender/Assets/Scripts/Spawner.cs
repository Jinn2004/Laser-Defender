using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject enemyPrefab;

	public float width = 5f;
	public float height = 5f;
	private bool moveRight = false;
	private float xMax;
	private float xMin;
	public float speed  = 2.5f;
	public float spawnDelay = 0.5f;
	
	// Use this for initialization
	void Start () {

        // this will limit the space the enemies can go.
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftEdge = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanceToCamera));
		Vector3 rightEdge = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanceToCamera));
		xMax = rightEdge.x - 1.25f;
		xMin = leftEdge.x + 1.25f;
		SpawnUntilFull();
		}

	void SpawnUntilFull(){
		Transform freePosition = NextFreePosition();
		if(freePosition){
			GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		if(NextFreePosition()){
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}

	void SpawnEnemies(){
		foreach (Transform child in transform) {
		//Instantiate (enemyPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
		// For each loop and will spawn for every child position.
			// This is to use the parent class stats, meaning I can set a formation for the enemies as they come
			// into play instead of having all of the enemies having indivual stats.
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}

	public void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position, new Vector3(width, height));
	}

	// Update is called once per frame
	void Update () {
		// This will move the positions to the right
		if (moveRight) {
			transform.position += Vector3.right * speed * Time.deltaTime;
			// another way to write this using the libary
			//transform.position += Vector3.right * speed * Time.deltaTime;
			//transform.position += new Vector3 (speed * Time.deltaTime, 0, 0);
		} else {
			transform.position += Vector3.left * speed * Time.deltaTime;
			//transform.position += Vector3.left * speed * Time.deltaTime;
			//transform.position += new Vector3 (-speed * Time.deltaTime, 0, 0);
		}


		float rightEdgeOfPosition = transform.position.x + (0.5f * width);
		float leftEdgeOfPosition = transform.position.x - (0.5f * width);

		/* This line works well enough even at high speeds.
		if (leftEdgeOfPosition <= xMin || rightEdgeOfPosition >= xMax) {
			moveRight = !moveRight;
		}
		*/
		// The instructor said this would fix the problem of the enemies going off of the map
		// however it makes mine go off the map no matter what the speed is.
		if (leftEdgeOfPosition < xMin) {
			moveRight = true;
		} else if(rightEdgeOfPosition > xMax){
			moveRight = false;
		}

		// check to see if all the enemies are gone
		if (AllMembersAreDead ()) {
			Debug.Log("Empty Formation");
			SpawnUntilFull();
		}

	} 

	// sets the next position and spawns enemies by a sequence instead of all at once
	Transform NextFreePosition(){
		foreach (Transform childPositionGameObject in transform) {
			if(childPositionGameObject.childCount == 0){
				return childPositionGameObject;
			}
		}
		return null;
	}


	bool AllMembersAreDead(){
		//transform.childCount; // counts how many children the parent has
		foreach (Transform childPositionGameObject in transform) {
			if(childPositionGameObject.childCount > 0){
				return false;
			}
		}
		return true;
	}
}
