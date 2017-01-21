using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomMoveTest : MonoBehaviour
{
	public float speed = 10;
	private bool goRight = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(transform.position.x > 10)
		{
			goRight = false;
		}
		else if(transform.position.x < -10)
		{
			goRight = true;
		}

		if(goRight)
		{
			transform.Translate(Vector3.right * speed / 10);
		}
		else
		{
			transform.Translate(Vector3.left * speed / 10);
		}

		
	}

	void Update()
	{
//		if(Input.GetKeyDown(KeyCode.P))
//		{
//			GetComponent<Rigidbody>().AddForce(Vector3.forward * 10, ForceMode.Impulse);
//		}
	}
}
