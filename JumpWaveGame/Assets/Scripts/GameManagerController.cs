using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.ThirdPerson;

using DG.Tweening;

public class GameManagerController : MonoBehaviour {
	// Menu canvases
	public Canvas StartScreenCanvas;
	public Canvas CountdownCanvas;
	public Canvas VictoryCanvas;
	public Canvas CreditsCanvas;

	public Camera GameCamera;
	public Camera IdleCamera;
	public Camera PlayerSelectCamera;

	private bool gameStarted;

	public float idleCameraRotationSpeed = 1.0f;

	private Tween startScreenFadeOutTween;

	public Material[] playerMaterials;

	public GameObject[] players;

	public GameObject PlayerSelect;

	private int activePlayerCount;

	private Transform lastStandingPlayer;

	private bool doSlowRotate = true;

	public float slowZoomSpeed = 1.0f;

	public float minimumPlayerDistance = 4.0f;

	public float minimumCameraXRotation = 15.0f;

	private bool canRestartGame = false;

	void Start () {
		Debug.Assert(CountdownCanvas, "GameManager needs a reference to the CountdownCanvas");
		Debug.Assert(StartScreenCanvas, "GameManager needs a reference to the StartScreenCanvas");
		Debug.Assert(GameCamera, "GameManager is missing a GameCamera");
		Debug.Assert(IdleCamera, "GameManager is missing an IdleCamera");
		Debug.Assert(players.Length == 8, "GameManager needs 8 Player objects");

		SetupStartScreen();

		SetupCameras();
	
		SceneManager.LoadScene("ArtTest", LoadSceneMode.Additive);
		SceneManager.LoadScene("GameElementsScene", LoadSceneMode.Additive);
	}

	void Update () {
		if (Input.anyKeyDown && !gameStarted) {
			FadeOutStartScreen();
		}

		if (IdleCamera.enabled) {
			IdleCamera.transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * idleCameraRotationSpeed);
		}

		if (doSlowRotate && lastStandingPlayer) {
			GameCamera.transform.RotateAround(lastStandingPlayer.position, Vector3.up, -Time.deltaTime * idleCameraRotationSpeed);
		}

		if (Input.GetKeyDown(KeyCode.F12)) {
			SceneManager.LoadScene("StartScene");
		}
	}

	public void RegisterPlayer(int playerIndex) {
		if (!players[playerIndex].activeSelf) {
			players[playerIndex].SetActive(true);
			activePlayerCount += 1;
		}
	}

	public void ReportDeath(GameObject ignoredPlayerArgument) {
		activePlayerCount -= 1;

		// TODO (Emil): This probably won't work with a double death. Wait a while before deciding?
		if (activePlayerCount == 1) {
			lastStandingPlayer = TryFindingLastStandingPlayerPosition();

			DoSlowZoom(lastStandingPlayer);
		}
		else if (activePlayerCount < 1) {
			GameCamera.enabled = false;
			IdleCamera.enabled = true;
		}
		else {
			Debug.Log("A player died");	
		}
	}

	private void DoSlowZoom(Transform player) {
		float zoomDuration = 5.0f;
		float heightOffset = -5.0f;
		float zOffset = 12.0f;
		float finalXRotation = 27.0f;

		Vector3 newCameraPosition = player.transform.position - new Vector3(0, heightOffset, zOffset);
		var animator = player.GetComponent<Animator>();
		animator.SetTrigger("Cheer");

		player.transform.LookAt(new Vector3(newCameraPosition.x, 0, newCameraPosition.z), Vector3.up);
		GameCamera.transform.DOMove(newCameraPosition, zoomDuration);
		GameCamera.transform.DORotate(new Vector3(finalXRotation, 0.0f, 0.0f), zoomDuration).OnComplete(StartCreditsSequence);
		doSlowRotate = true;

		StartCreditsSequence();
	}

	private void StartCreditsSequence() {
		float creditsDelay = 5.0f;
		float fadeInDuration = 2.0f;
		var canvasRenderer = CreditsCanvas.GetComponent<CanvasRenderer>();

		Sequence creditsSequence = DOTween.Sequence();
		creditsSequence.AppendInterval(creditsDelay);
		creditsSequence.Append(DOTween.ToAlpha(canvasRenderer.GetColor, canvasRenderer.SetColor, 1.0f, fadeInDuration));
	}

	private void SetupCameras() {
		GameCamera.enabled = false;
		IdleCamera.enabled = false;
		PlayerSelectCamera.enabled = true;
	}

	private void SetupStartScreen() {
		StartScreenCanvas.gameObject.SetActive(true);

		var canvasGroup = StartScreenCanvas.GetComponent<CanvasGroup>();
		
		startScreenFadeOutTween = canvasGroup.DOFade(0.0f, 1.0f);
		startScreenFadeOutTween.Pause();
		startScreenFadeOutTween.OnComplete(MoveToGameLobby);
	}

	private void GoToIdleCamera() {
		GameCamera.enabled = false;
	}

	private void FadeOutStartScreen() {
		startScreenFadeOutTween.Rewind();
		startScreenFadeOutTween.Restart();

		// TODO (Emil): This should probably be called something else because of menu sequence changes.
		gameStarted = true;
	}

	private void MoveToGameLobby() {
		StartScreenCanvas.gameObject.SetActive(false);
	}

	public void StartGame() {
		PlayerSelectCamera.enabled = false;
		GameCamera.enabled = true;

		CountdownCanvas.gameObject.SetActive(true);

		//lastStandingPlayer = TryFindingLastStandingPlayerPosition();
		//DoSlowZoom(lastStandingPlayer);
	}

	private Transform TryFindingLastStandingPlayerPosition() {
		foreach (var player in players) {
			if (player.gameObject.activeSelf) {
				return player.transform;
			}
		}

		return null;
	}

	private void EndGame() {
	}
}
