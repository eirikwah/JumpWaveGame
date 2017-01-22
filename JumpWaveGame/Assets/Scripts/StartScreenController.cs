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
	}
	
	private void DoFadeIn() {
		fadeInClickToPlay.Rewind();
		fadeInClickToPlay.Restart();
	}

	
}
