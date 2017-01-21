using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerHitSoundPlayer : MonoBehaviour {
    [FMODUnity.EventRef]
    public string KickHitSound = "event:/KickHit";

    [FMODUnity.EventRef]
    public string WaveHitSound = "event:/Hit";

    public void OnCollisionEnter(Collision collision) {
        //Debug.Log("Hit collider " + collision.gameObject.name);
        if (collision.gameObject.name.Equals("AttackCollider"))
        {
            RuntimeManager.PlayOneShot(KickHitSound, Vector3.zero);
        }
        else if (collision.gameObject.name.StartsWith("Wave"))
        {
            RuntimeManager.PlayOneShot(WaveHitSound, Vector3.zero);
        }
        else
        {
            Debug.Log("No sound for hitting collider " + collision.gameObject.name);
        }
    }

    public void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.name.Equals("AttackCollider"))
        {
            RuntimeManager.PlayOneShot(KickHitSound, Vector3.zero);
        }
        else if (collider.gameObject.name.StartsWith("Wave"))
        {
            RuntimeManager.PlayOneShot(WaveHitSound, Vector3.zero);
        }
        else
        {
            Debug.Log("No sound for hitting trigger " + collider.gameObject.name);
        }
    }

}
