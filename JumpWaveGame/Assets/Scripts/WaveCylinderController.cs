using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WaveCylinderController : MonoBehaviour {
    private WaveController parentWaveController;
	private string waveNumberAsString;
	
	private float xzStrength = 2f;
	private float yStrength = 27f;

	private List<GameObject> playerHit = new List<GameObject>();

    public void Start() {
        parentWaveController = transform.parent.gameObject.GetComponent<WaveController>();
        Assert.IsNotNull(parentWaveController, "Parent of WaveCylinderController must be WaveController");

		// Layer name is typically "Wave3":
		//		waveNumberAsString = LayerMask.LayerToName(transform.parent.gameObject.layer).Substring(4);

		//Did this so I didn't have to change the layers on the Wave Parents (they are set to default)
		waveNumberAsString = transform.parent.name.Substring(4); 
    }

	public void OnCollisionEnter(Collision collision)
	{
		if(!collision.gameObject.name.Contains("Player"))
		{
			return;
		}

		var rb = collision.rigidbody;
		if(rb != null)
		{
			string otherLayerName = LayerMask.LayerToName(collision.gameObject.layer);
			// waveNumberAsString may be uninitialized if this wave was just created
			if(waveNumberAsString == null || otherLayerName.EndsWith(waveNumberAsString) || playerHit.Contains(rb.gameObject))
			{
				// If the collision is with the player that sent out this wave, then ignore it
				return;
			}

			Debug.Log(waveNumberAsString + "   " + otherLayerName + "  " + otherLayerName.EndsWith(waveNumberAsString));

			Vector3 dir = (parentWaveController.transform.position - collision.transform.position).normalized;

			rb.velocity = Vector3.zero; //Resets the velocity of the rigidbody right before we add the force
			rb.AddForce(-(parentWaveController.gameObject.transform.position - new Vector3(collision.gameObject.transform.position.x * xzStrength, yStrength, collision.gameObject.transform.position.z * xzStrength)), ForceMode.Impulse);
			
			playerHit.Add(rb.gameObject);
		}
	}

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("PlayfieldEdge")) {
            parentWaveController.OnCylinderLeftPlayfield();
        }
    }

	public void OnDisable()
	{
		playerHit.Clear();
	}

}
