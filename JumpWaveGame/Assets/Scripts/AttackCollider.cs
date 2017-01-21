using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
	public float pushbackLengthForce;
	public float pushbackHeightForce;
	private Vector3 force;

	void OnTriggerEnter(Collider col)
	{
//		if(col.gameObject.layer == LayerMask.NameToLayer)
		var rb = col.gameObject.GetComponent <Rigidbody>();
		if(rb != null)
		{
			rb.velocity = Vector3.zero;
			rb.AddForce(-(transform.position - new Vector3(col.gameObject.transform.position.x, pushbackHeightForce, col.gameObject.transform.position.z)) * pushbackLengthForce, ForceMode.Impulse);
			Debug.Log("Attacking object " + rb.gameObject.name);
		}
	}
}
