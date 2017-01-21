using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class InputManager : MonoBehaviour
{
	[SerializeField]
	private ThirdPersonCharacter thirdPersonCharacter;

	private string layerName;
	private bool jumpBool;
	private bool attackBool;
	// Use this for initialization
	private void Start ()
	{
		thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
		Debug.Log("Layer name is " + LayerMask.LayerToName(gameObject.layer));
		layerName = LayerMask.LayerToName(gameObject.layer);
	}

	private void Update()
	{
		if(Input.GetButtonDown(layerName + "Jump"))
		{
			jumpBool = true;
		}

		if(Input.GetButtonDown(layerName + "Fire1"))
		{
			attackBool = true;
		}
	}

	// Update is called once per frame
	private void FixedUpdate () {
		float h = Input.GetAxis(layerName + "Horizontal");
		float v = Input.GetAxis(layerName + "Vertical");

		Vector3 moveVector = v* Vector3.forward + h * Vector3.right;

		thirdPersonCharacter.ReceiveInput(moveVector, attackBool, jumpBool);
		attackBool = false;
		jumpBool = false;
	}
}
