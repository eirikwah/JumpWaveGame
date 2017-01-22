using UnityEngine;
using DG.Tweening;

public class PlayerSelectController : MonoBehaviour {
	public float playerSelectionDelay = 2.0f;

	public GameObject[] PlayerSelectModels;

	private GameManagerController gameManager;

	private bool[] activePlayers;

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
		if (playerSelectionDelay > 0.0f) {
			playerSelectionDelay -= Time.deltaTime;
		}
		else
		{
			for (int playerIndex = 0; playerIndex < playerCount; playerIndex++) {
				if (Input.GetButtonDown("Player" + (playerIndex + 1).ToString() + "Jump") && !activePlayers[playerIndex]) {
					gameManager.RegisterPlayer(playerIndex);
					PlayerSelectModels[playerIndex].SetActive(true);
					// NOTE (Emil): We use the activePlayerCount for the starting logic below.
					activePlayerCount += 1;
				}
			}

			// TODO (Emil): Make this another button?
			if (Input.GetKey(KeyCode.JoystickButton7)) {
				Debug.Log("Is Button7 start?");
			}

			if (Input.GetKey(KeyCode.JoystickButton7) && activePlayerCount > 0) {
				Debug.Log("Pressed space and more than zero active players");

				gameManager.StartGame();
				gameObject.SetActive(false);
			}
		}
	}
}
