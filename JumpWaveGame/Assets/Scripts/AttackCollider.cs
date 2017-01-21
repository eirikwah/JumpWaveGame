using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
	public float pushbackLengthForce;
	public float pushbackHeightForce;
	private Vector3 force;
	// Use this for initialization
	void Start () {
		force = new Vector3(0,pushbackHeightForce,0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col)
	{
//		if(col.gameObject.layer == LayerMask.NameToLayer)
		var rb = col.gameObject.GetComponent <Rigidbody>();
		if(rb != null)
		{
			Vector3 forcePositionVector = col.contacts[0].point - transform.position;
			forcePositionVector = -forcePositionVector.normalized;
			rb.AddForce(-(transform.position - new Vector3(col.gameObject.transform.position.x, pushbackHeightForce, col.gameObject.transform.position.z)) * pushbackHeightForce, ForceMode.Impulse);
			Debug.Log("Attacking object " + rb.gameObject.name);
		}

		Debug.Log("Attacking object " + rb.gameObject.name);

	}
}
