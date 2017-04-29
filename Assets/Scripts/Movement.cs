using UnityEngine;
using System.Collections;
using System;

public class Movement : MonoBehaviour {

	enum direction { UP, DOWN, LEFT, RIGHT };
	public enum playerNumber { ONE, TWO, THREE, FOUR };

	[HideInInspector]
	public Vector3 originalPosition;
	public playerNumber myNumber;
	public Color color;
	//public Material material;
	public event Action deathEvent;

	direction myDirection;
	Vector3 moveDirection;
	Vector3 rayOrigin;
	Renderer playerRenderer;
	MeshRenderer meshRenderer;
	//ParticleSystem particles;
	Ray ray;
	Ray playerDetection;
	string [] inputStrings;
	int lives;
	bool once;
	bool lostLife;
	bool cooldown;
	bool canFall;

	// Use this for initialization
	void Start () {
		playerDetection = new Ray ();
		moveDirection = transform.position;
		originalPosition = transform.position;
		rayOrigin = transform.position;
		ray = new Ray ();
		once = true;
		cooldown = false;
		playerRenderer = GetComponent<Renderer> ();
		meshRenderer = GetComponent <MeshRenderer> ();
		//particles = GetComponentInChildren <ParticleSystem> ();
		//material.color = color;
		playerRenderer.material.color = color;
		lives = 3;
		lostLife = false;
		canFall = true;
		inputStrings = new string[5];
		if (myNumber == playerNumber.ONE) {
			inputStrings [0] = "w";
			inputStrings [1] = "s";
			inputStrings [2] = "a";
			inputStrings [3] = "d";
			inputStrings [4] = "space";
		} else if (myNumber == playerNumber.TWO) {
			inputStrings [0] = "up";
			inputStrings [1] = "down";
			inputStrings [2] = "left";
			inputStrings [3] = "right";
			inputStrings [4] = "return";
		} 
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y == 1.5f) {
			Move ();
			Attack ();
		} else if (transform.position.y < 1.5f && !lostLife) {
			lives--;
			if (deathEvent != null) {
				deathEvent ();
			}
			Invoke ("Respawn", 3.0f);
			lostLife = true;
		}
		if (!canFall) {
			gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		} else if (canFall) {
			gameObject.GetComponent<Rigidbody> ().isKinematic = false;
		}
	}

	IEnumerator Blink () {

		yield return new WaitForSeconds (0.2f);
		meshRenderer.enabled = false;
		yield return new WaitForSeconds (0.2f);
		meshRenderer.enabled = true;
		yield return new WaitForSeconds (0.2f);
		meshRenderer.enabled = false;
		yield return new WaitForSeconds (0.2f);
		meshRenderer.enabled = true;
		canFall = true;
		yield return new WaitForEndOfFrame ();
	}

	void Move () {
		rayOrigin = transform.position;
		rayOrigin.y -= 1.5f;
		bool hit;
		bool playerHit;
		ray.origin = rayOrigin;
		originalPosition = transform.position;
		playerDetection.origin = transform.position;
		if (Input.GetKeyDown (inputStrings[0])) {
			ray.direction = transform.forward;
			playerDetection.direction = transform.forward;
			myDirection = direction.UP;
			once = true;
		} else if (Input.GetKeyDown (inputStrings[1])) {
			ray.direction = -transform.forward;
			playerDetection.direction = transform.forward;
			myDirection = direction.DOWN;
			once = true;
		} else if (Input.GetKeyDown (inputStrings[2])) {
			ray.direction = -transform.right;
			playerDetection.direction = transform.forward;
			myDirection = direction.LEFT;
			once = true;
		} else if (Input.GetKeyDown (inputStrings[3])) {
			ray.direction = transform.right;
			playerDetection.direction = transform.forward;
			myDirection = direction.RIGHT;
			once = true;
		}
		if (once) {
			hit = Physics.Raycast (ray.origin, ray.direction, 1.0f);
			playerHit = Physics.Raycast (playerDetection.origin, playerDetection.direction, 1.0f);
			if (hit && !playerHit) {
				if (myDirection == direction.UP) {
					moveDirection.z = transform.position.z + 1.1f;
				} else if (myDirection == direction.DOWN) {
					moveDirection.z = transform.position.z - 1.1f;
				} else if (myDirection == direction.LEFT) {
					moveDirection.x = transform.position.x - 1.1f;
				} else if (myDirection == direction.RIGHT) {
					moveDirection.x = transform.position.x + 1.1f;
				}
				transform.position = moveDirection;
			}
			once = false;
		}
	}

	void Attack () {
		rayOrigin = transform.position;
		rayOrigin.y = transform.position.y - 1.5f;
		RaycastHit[] hits;
		ray.origin = rayOrigin;
		if (Input.GetKeyDown (inputStrings[4]) && !cooldown) {
			//particles.Play ();
			if (myDirection == direction.UP) {
				ray.direction = transform.forward;
			} else if (myDirection == direction.DOWN) {
				ray.direction = -transform.forward;
			} else if (myDirection == direction.LEFT) {
				ray.direction = -transform.right;
			} else if (myDirection == direction.RIGHT) {
				ray.direction = transform.right;
			}
			hits = Physics.RaycastAll (ray, 100.0f); 
			for (int i = 0; i < hits.Length; i++) {
				if (hits [i].transform.GetComponent<FallingBlocks> () != null) {
					hits [i].transform.GetComponent<FallingBlocks> ().Shake ();
					hits [i].transform.GetComponent<FallingBlocks> ().blockRenderer.material.color = color;
				}
			}
			cooldown = true;
			Invoke ("Cooldown", 1.0f);
		}
	}

	public int GetLives () {
		return lives;
	}

	void Cooldown () {
		cooldown = false;
	}

	void Respawn () {
		if (lives > 0) {
			lostLife = false;
			transform.position = originalPosition;
			canFall = false;
			StartCoroutine (Blink ());
		}
	}
}
