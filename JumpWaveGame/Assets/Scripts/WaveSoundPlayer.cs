using UnityEngine;
using FMODUnity;

public class WaveSoundPlayer : MonoBehaviour {
    [FMODUnity.EventRef]
    public string WaveSound = "event:/WaveImpactSound";

	// Use this for initialization
	public void Start () {
        RuntimeManager.PlayOneShot(WaveSound, Vector3.zero);
	}
	
}
