using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

using DG.Tweening;

public class CanvasController : MonoBehaviour {
	public float FullCountdownDuration;

	public float AnimationStartDelay = 0.0f;

	private Sequence animationSequence;

	private Transform players;

	void Start() {
		Debug.Assert(transform.childCount > 0, "Missing CountdownItems for the Canvas");
		Debug.Assert(FullCountdownDuration > 0.0f, "The Countdown duration must be larger than zero.");

		players = GameObject.Find("Players").transform;

		Debug.Assert(players, "Need players");

		float duration = FullCountdownDuration / transform.childCount;

		// NOTE (Emil): Create the animation sequence and an initial pause.
		animationSequence = DOTween.Sequence();
		animationSequence.AppendInterval(AnimationStartDelay);

		foreach (Transform transform in transform) {
			CanvasRenderer renderer = transform.GetComponent<CanvasRenderer>();

			renderer.SetAlpha(0.0f);

			animationSequence.Append(DOTween.ToAlpha(renderer.GetColor, renderer.SetColor, 1.0f, 0.1f));
			animationSequence.Append(transform.DOScale(new Vector3(2, 2, 1), duration - 0.1f));
			animationSequence.Append(DOTween.ToAlpha(renderer.GetColor, renderer.SetColor, 0.0f, duration));
		}

		animationSequence.OnComplete(() => {
			foreach(Transform player in players) {
				var character = player.GetComponent<ThirdPersonCharacter>();
				character.gameOver = false;
			}

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

		animationSequence.Rewind();
		animationSequence.Restart();
	}
}
