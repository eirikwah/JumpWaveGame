using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class InputManager : MonoBehaviour
{
	[SerializeField]
	private ThirdPersonCharacter thirdPersonCharacter;

	private bool jumpBool;
	private bool attackBool;
	// Use this for initialization
	private void Start ()
	{
		thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
	}

	private void Update()
	{
		if(!jumpBool)
		{
			jumpBool = Input.GetButtonDown("Jump");
		}

		if(!attackBool)
		{
			attackBool = Input.GetButtonDown("Fire1");
		}
	}

	// Update is called once per frame
	private void FixedUpdate () {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		Vector3 moveVector = v* Vector3.forward + h * Vector3.right;

		thirdPersonCharacter.ReceiveInput(moveVector, attackBool, jumpBool);
		attackBool = false;
		jumpBool = false;
	}
}
