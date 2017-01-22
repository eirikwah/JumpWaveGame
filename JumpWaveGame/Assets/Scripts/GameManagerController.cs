using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using DG.Tweening;

public class GameManagerController : MonoBehaviour {
	// Menu canvases
	public Canvas StartScreenCanvas;
	public Canvas CountdownCanvas;
	public Canvas VictoryCanvas;
	public Canvas CreditsCanvas;
	public Canvas SignupCanvas;

	public Camera GameCamera;
	public Camera IdleCamera;
	public Camera PlayerSelectCamera;

	private bool gameStarted;

	public float idleCameraRotationSpeed = 1.0f;

	private Tween startScreenFadeOutTween;

	public Material[] playerMaterials;

	public GameObject[] players;

	void Start () {
		Debug.Assert(CountdownCanvas, "GameManager needs a reference to the CountdownCanvas");
		Debug.Assert(StartScreenCanvas, "GameManager needs a reference to the StartScreenCanvas");
		Debug.Assert(GameCamera, "GameManager is missing a GameCamera");
		Debug.Assert(IdleCamera, "GameManager is missing an IdleCamera");
		Debug.Assert(players.Length == 8, "GameManager needs 8 Player objects");

		SetupStartScreen();

		SetupCameras();
	
		SceneManager.LoadScene("ArtTest", LoadSceneMode.Additive);
	}

	void Update () {
		if (Input.anyKeyDown && !gameStarted) {
			FadeOutStartScreen();
		}

		if (IdleCamera.enabled) {
			IdleCamera.transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * idleCameraRotationSpeed);
		}
	}

	public void RegisterPlayer(int playerIndex) {
		players[playerIndex].SetActive(true);
	}

	public void ReportDeath(GameObject player) {
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
		startScreenFadeOutTween.OnComplete(StartGame);
	}

	private void FadeOutStartScreen() {
		startScreenFadeOutTween.Rewind();
		startScreenFadeOutTween.Restart();

		// TODO (Emil): This should probably be called something else because of menu sequence changes.
		gameStarted = true;
	}

	private void MoveToSignupMenu() {
		SignupCanvas.gameObject.SetActive(true);
		StartScreenCanvas.gameObject.SetActive(false);
	}

	public void StartGame() {
		StartScreenCanvas.gameObject.SetActive(false);
		SignupCanvas.gameObject.SetActive(false);
	}
}
