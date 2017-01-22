using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePlayerPositionAndSetInactive : MonoBehaviour {
	private Vector3 startPosition;
	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		gameObject.SetActive(false);
	}

	void OnEnable() {
		
	}
}
