using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeInDelay : MonoBehaviour {

	public float fadeInDelay;

	public float fadeDuration;

	private Sequence fadeInSequence;

	private CanvasRenderer canvasRenderer;

	// Use this for initialization
	void Start () {
		canvasRenderer = gameObject.GetComponent<CanvasRenderer>();
		Debug.Assert(canvasRenderer, "FadeInDelay needs to be attached to an object with a CanvasRenderer");

		canvasRenderer.SetAlpha(0.0f);

		fadeInSequence = DOTween.Sequence();
		fadeInSequence.Pause();

		fadeInSequence.AppendInterval(fadeInDelay);
		fadeInSequence.Append(DOTween.ToAlpha(canvasRenderer.GetColor, canvasRenderer.SetColor, 1.0f, fadeDuration));
	}

	void OnEnable() {
		Debug.Log("FadeInDelay.OnEnable");

		canvasRenderer.SetAlpha(0.0f);

		fadeInSequence.Rewind();
		fadeInSequence.Restart();
	}
}
