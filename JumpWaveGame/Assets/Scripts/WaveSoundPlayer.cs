using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSoundPlayer : MonoBehaviour {
    [FMODUnity.EventRef]
    public string WaveSound = "event:/WaveImpact";

	// Use this for initialization
	public void Start () {
        FMODUnity.RuntimeManager.PlayOneShot(WaveSound, Vector3.zero);
	}
	
}
