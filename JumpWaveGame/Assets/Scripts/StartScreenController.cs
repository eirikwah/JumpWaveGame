using UnityEngine;
using DG.Tweening;

public class StartScreenController : MonoBehaviour {
	public GameObject ClickToPlayText;

	public float FadeInDelay;
	public float FadeInDuration;
	private Sequence fadeInClickToPlay;

	private GameManagerController gameManager;

	// Use this for initialization
	void Start () {
		var renderer = ClickToPlayText.GetComponent<CanvasRenderer>();
		var textMesh = ClickToPlayText.GetComponent<TextMesh>();
		Debug.Assert(renderer, "StartScreenController needs a clickToPlayText with a CanvasRenderer.");
		Debug.Assert(FadeInDuration > 0.0f, "StartScreenController needs a positive FadeInDuration");

		renderer.SetAlpha(0.0f);

		fadeInClickToPlay = DOTween.Sequence();
		fadeInClickToPlay.Pause();

		fadeInClickToPlay.AppendInterval(FadeInDelay);
		fadeInClickToPlay.Append(DOTween.ToAlpha(renderer.GetColor, renderer.SetColor, 1.0f, FadeInDuration));
	}

	void OnEnable() {
		Debug.Log("StartScreenController.OnEnable");

		DoFadeIn();
	}

	void Update() {
		CheckPlayerInputForRegistration();
	}
	
	private void DoFadeIn() {
		fadeInClickToPlay.Rewind();
		fadeInClickToPlay.Restart();
	}

	private void CheckPlayerInputForRegistration() {
		if (Input.GetKey("Player1Jump")) {
			gameManager.RegisterPlayer(0);
		}

		if (Input.GetKey("Player2Jump")) {
			gameManager.RegisterPlayer(1);
		}

		if (Input.GetKey("Player3Jump")) {
			gameManager.RegisterPlayer(2);
		}

		if (Input.GetKey("Player4Jump")) {
			gameManager.RegisterPlayer(3);
		}

		if (Input.GetKey("Player5Jump")) {
			gameManager.RegisterPlayer(4);
		}

		if (Input.GetKey("Player6Jump")) {
			gameManager.RegisterPlayer(5);
		}

		if (Input.GetKey("Player7Jump")) {
			gameManager.RegisterPlayer(6);
		}

		if (Input.GetKey("Player8Jump")) {
			gameManager.RegisterPlayer(7);
		}
	}
}
