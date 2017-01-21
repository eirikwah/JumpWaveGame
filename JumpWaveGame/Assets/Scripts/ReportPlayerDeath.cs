using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportPlayerDeath : MonoBehaviour {
	private GameManagerController gameManager;

	public float killAtYCoordinate = -2.0f;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManagerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < killAtYCoordinate) {
			gameManager.ReportDeath(transform.parent.gameObject);
			gameObject.SetActive(false);
		}
	}
}
