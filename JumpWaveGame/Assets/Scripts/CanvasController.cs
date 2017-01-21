using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class CanvasController : MonoBehaviour {
	public float FullCountdownDuration;

	private Sequence animationSequence;

	void Start() {
		Debug.Assert(transform.childCount > 0, "Missing CountdownItems for the Canvas");
		Debug.Assert(FullCountdownDuration > 0.0f, "The Countdown duration must be larger than zero.");

		float duration = FullCountdownDuration / transform.childCount;

		// Create the animation sequence and an initial pause.
		animationSequence = DOTween.Sequence();
		animationSequence.AppendInterval(0.5f);

		foreach (Transform transform in transform) {
			CanvasRenderer renderer = transform.GetComponent<CanvasRenderer>();

			animationSequence.Append(DOTween.ToAlpha(renderer.GetColor, renderer.SetColor, 1.0f, 0.1f));
			animationSequence.Append(transform.DOScale(new Vector3(2, 2, 1), duration - 0.1f));
			animationSequence.Append(DOTween.ToAlpha(renderer.GetColor, renderer.SetColor, 0.0f, duration));
		}


		animationSequence.OnComplete(() => {
			animationSequence.Rewind();
			gameObject.SetActive(false);
		});
	}

	void SetChildrenAsInvisible() {
		foreach (Transform transform in transform) {
			CanvasRenderer renderer = transform.GetComponent<CanvasRenderer>();
			renderer.SetAlpha(0.0f);
		}
	}

	void OnEnable() {
		StartCountdown();
	}

	private void StartCountdown() {
		SetChildrenAsInvisible();

		animationSequence.Restart();
	}
}
