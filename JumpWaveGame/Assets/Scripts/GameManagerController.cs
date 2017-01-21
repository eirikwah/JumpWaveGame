using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class GameManagerController : MonoBehaviour {
	public Canvas CountdownCanvas;

	public Canvas StartScreenCanvas;

	public float fadeOutDuration;

	void Start () {
		Debug.Assert(CountdownCanvas, "GameManager needs a reference to the CountdownCanvas");
		Debug.Assert(StartScreenCanvas, "GameManager needs a reference to the StartScreenCanvas");

		StartScreenCanvas.gameObject.SetActive(true);
		var renderer = StartScreenCanvas.GetComponent<CanvasRenderer>();

		DOTween.ToAlpha(renderer.GetColor, renderer.SetColor, 0.0f, 1.0f);
	}

	void Update () {
		
	}

	void OnEnable() {
	}
}
