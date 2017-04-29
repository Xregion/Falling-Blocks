using UnityEngine;
using System.Collections;

public class FallingBlocks : MonoBehaviour {

	[HideInInspector]
	public Renderer blockRenderer;

	Vector3 respawnPostion;
	Quaternion respawnRotation;
	bool isFalling;
	bool isSpinning;

	// Use this for initialization
	void Start () {
		isFalling = false;
		isSpinning = false;
		respawnPostion = transform.position;
		respawnRotation = transform.rotation;
		blockRenderer = GetComponent<Renderer> ();
	}

	IEnumerator Spin () {

		isSpinning = true;

		while (transform.rotation.y < 0.2f) {
			transform.Rotate (new Vector3 (0.0f, 2.0f, 0.0f));

			yield return null;
		}

		while (transform.rotation.y > -0.2f) {
			transform.Rotate (new Vector3 (0.0f, -2.0f, 0.0f));

			yield return null;
		}

		Fall ();
		yield return new WaitForEndOfFrame ();
	}

	void FixedUpdate () {
		if (isFalling) {
			transform.Translate (new Vector3 (0.0f, -Time.deltaTime, 0.0f));
		}
	}

	public void Shake () {
		if (!isFalling && !isSpinning) {
			StartCoroutine (Spin ());
		}
	}
	
	void Fall () {
		isFalling = true;
		Invoke ("Respawn", 3.0f);
	}

	void Respawn () {
		blockRenderer.material.color = Color.white;
		transform.position = respawnPostion;
		transform.rotation = respawnRotation;
		isFalling = false;
		isSpinning = false;
	}
}
