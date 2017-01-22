using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectController : MonoBehaviour {

	private GameManagerController gameManager;

	private bool[] activePlayers;

	public GameObject[] PlayerSelectModels;

	private int playerCount = 8;

	private int activePlayerCount = 0;
	// Use this for initialization
	void Start () {
		activePlayers = new bool[playerCount];

		Debug.Assert(activePlayerCount == 0, "There should be no active players at startup.");
		Debug.Assert(PlayerSelectModels.Length == playerCount, "PlayerSelect needs 8 player models");
		Debug.Assert(activePlayers.Length == playerCount, "PlayerSelect needs 8 player models");
		gameManager = GameObject.Find("GameManager").GetComponent<GameManagerController>();

		Debug.Assert(gameManager, "Failed to find the GameManager Object");

		for (int i = 0; i < playerCount; i++) {
			activePlayers[i] = false;
		}

		// TODO (Emil): Remove this since we want the characters on the screen at all times.
		// For Safety.
		foreach (var playerModel in PlayerSelectModels) {
			playerModel.gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckPlayerInputForRegistration();
	}
	private void CheckPlayerInputForRegistration() {
		for (int playerIndex = 0; playerIndex < playerCount; playerIndex++) {
			if (Input.GetButtonDown("Player" + (playerIndex + 1).ToString() + "Jump") && !activePlayers[playerIndex]) {
				gameManager.RegisterPlayer(playerIndex);
				PlayerSelectModels[playerIndex].SetActive(true);
			}
		}

		if (Input.GetKey("space") && activePlayerCount > 0) {
			gameManager.StartGame();
			gameObject.SetActive(false);
		}
	}
}
