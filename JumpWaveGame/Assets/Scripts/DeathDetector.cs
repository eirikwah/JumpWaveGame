using UnityEngine;
using FMODUnity;

public class DeathDetector : MonoBehaviour {
    [FMODUnity.EventRef]
    public string VictorySound = "event:/Victory";

    [FMODUnity.EventRef]
    public string DeathByFallingSound = "event:/WilhelmScream";

    public void OnCollisionExit(Collision collision) {
        var player = collision.gameObject;
        CheckGameobjectLeavingLifeCube(player, "collision exit");
    }

    public void OnTriggerExit(Collider collider) {
        var player = collider.gameObject;
        CheckGameobjectLeavingLifeCube(player, "trigger exit");
    }

    private void CheckGameobjectLeavingLifeCube(GameObject player, string debugText) {
        Debug.Log("Went outside the life cube by " + debugText + "; " + player.name);
        if (player.name.StartsWith("Player"))
        {
            // If this really is a player:
            RuntimeManager.PlayOneShot(DeathByFallingSound, Vector3.zero);

            player.SetActive(false);
        }
        else
        {
            Debug.Log("Ignoring non-player falling outside the life cube: " + player.name);
        }

    }
}
