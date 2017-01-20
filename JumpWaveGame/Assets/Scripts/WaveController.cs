using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.ComponentModel;

public class WaveController : MonoBehaviour {
    public Transform[] cylinders; 

    public float RadiusToLengthFactor;

    public float CurrentWaveRadius;

    public float RadiusExpansionRate = 1;

    // Private variables:
    public float angleDelta;

	public void Start () {
        Assert.IsNotNull(cylinders, "cylinders must be initialized");
        Assert.AreEqual(16, cylinders.Length);

        angleDelta = 360.0f / cylinders.Length;

        // Reset all positions:
        for (int i = 0; i < cylinders.Length; i++) {
            Transform cylinder = cylinders[i];
            cylinder.localPosition = Vector3.zero;
            cylinder.localEulerAngles = new Vector3(90, -i * angleDelta, 0);
            cylinder.localScale = new Vector3(1, 0, 1);
        }
	}
	
    public void Update () {

    }

    public void FixedUpdate () {
        CurrentWaveRadius += RadiusExpansionRate * Time.fixedDeltaTime;

        for (int i = 0; i < cylinders.Length; i++) {
            Transform cylinder = cylinders[i];
            cylinder.localPosition = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angleDelta * i) * CurrentWaveRadius, 0, Mathf.Sin(Mathf.Deg2Rad * angleDelta * i) * CurrentWaveRadius);
            cylinder.localScale = new Vector3(1, CurrentWaveRadius * RadiusToLengthFactor + Mathf.Sin(Mathf.Deg2Rad * angleDelta / 2.0f) * 0.5f, 1);
        }

    }


}
