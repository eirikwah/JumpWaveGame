using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class CanvasController : MonoBehaviour {
	public float FullCountdownDuration;

	void Start() {
		Debug.Assert(transform.childCount > 0, "Missing CountdownItems for the Canvas");
		Debug.Assert(FullCountdownDuration > 0.0f, "The Countdown duration must be larger than zero.");

		foreach (Transform transform in transform) {
			CanvasRenderer renderer = transform.GetComponent<CanvasRenderer>();

			renderer.SetAlpha(0.0f);
		}
	}

	void OnEnable() {
		StartCountdown();
	}

	private void StartCountdown() {
		float duration = FullCountdownDuration / transform.childCount;

		Sequence sequence = DOTween.Sequence();
		sequence.AppendInterval(0.5f);

		int transformIndex = 0;
		foreach (Transform transform in transform) {
			var renderer = transform.GetComponent<CanvasRenderer>();

			sequence.Append(DOTween.ToAlpha(renderer.GetColor, renderer.SetColor, 1.0f, 0.2f));
			sequence.Append(transform.DOScale(new Vector3(2, 2, 1), duration));
			sequence.PrependInterval(transformIndex * duration - 0.2f);
			sequence.Append(DOTween.ToAlpha(renderer.GetColor, renderer.SetColor, 0.0f, duration));
			sequence.OnComplete(() => gameObject.SetActive(false));
		}
	}
}
