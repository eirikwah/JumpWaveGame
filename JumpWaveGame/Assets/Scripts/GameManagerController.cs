using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class GameManagerController : MonoBehaviour {
	public Canvas CountdownCanvas;

	public Canvas StartScreenCanvas;

	public float fadeOutDuration;

	private Tween startScreenFadeOutTween;

	void Start () {
		Debug.Assert(CountdownCanvas, "GameManager needs a reference to the CountdownCanvas");
		Debug.Assert(StartScreenCanvas, "GameManager needs a reference to the StartScreenCanvas");

		var canvasGroup = StartScreenCanvas.GetComponent<CanvasGroup>();
		
		startScreenFadeOutTween = canvasGroup.DOFade(0.0f, 1.0f);
		startScreenFadeOutTween.Pause();
		// startScreenFadeOutTween.Rewind();
		startScreenFadeOutTween.OnComplete(StartGame);
	}

	void StartGame() {
		CountdownCanvas.gameObject.SetActive(true);
		StartScreenCanvas.gameObject.SetActive(false);
	}

	void Update () {
		if (Input.anyKeyDown) {
			Debug.Log("Pressed any button!");

			startScreenFadeOutTween.Rewind();
			startScreenFadeOutTween.Restart();
		}
	}
}
