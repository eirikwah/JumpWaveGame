using UnityEngine;
using UnityEngine.Assertions;

public class WaveCylinderController : MonoBehaviour {
    private WaveController parentWaveController;

    public void Start() {
        parentWaveController = transform.parent.gameObject.GetComponent<WaveController>();
        Assert.IsNotNull(parentWaveController, "Parent of WaveCylinderController must be WaveController");
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("PlayfieldEdge")) {
            parentWaveController.OnCylinderLeftPlayfield();
        }
    }

}
