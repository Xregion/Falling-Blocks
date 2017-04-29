using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lives : MonoBehaviour {

	public Text one;
	public Text two;
	public Text three;
	public Text four;

	Movement [] players;

	// Use this for initialization
	void Start () {
		players = FindObjectsOfType<Movement> ();
		for (int i = 0; i < players.Length; i++) {
			players [i].deathEvent += Died;
		}
		if (players.Length < 2) {
			two.enabled = false;
			three.enabled = false;
			four.enabled = false;
		} else if (players.Length == 2) {
			three.enabled = false;
			four.enabled = false;
		} else if (players.Length == 3) {
			four.enabled = false;
		}
	}
	
	void Died () {
		for (int i = 0; i < players.Length; i++) {
			if (players [i].myNumber == Movement.playerNumber.ONE) {
				one.text = "Player One: " + players [i].GetLives () + " Lives";
			} else if (players [i].myNumber == Movement.playerNumber.TWO) {
				two.text = "Player Two: " + players [i].GetLives () + " Lives";
			} else if (players [i].myNumber == Movement.playerNumber.THREE) {
				three.text = "Player Three: " + players [i].GetLives () + " Lives";
			} else if (players [i].myNumber == Movement.playerNumber.FOUR) {
				four.text = "Player Four: " + players [i].GetLives () + " Lives";
			}
		}
	}
}
