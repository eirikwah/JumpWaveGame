using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;
using System.Runtime.InteropServices;

public class WaveController : MonoBehaviour {
    public Transform CylinderPrefab; 
    public Material WaveMaterial;

    public float CylinderDiameter = 1;
    public int NumberOfCylinders = 16;
    public float RadiusExpansionRate = 3;

    private Transform[] cylinders; 

    [SerializeField]
    private float CurrentWaveRadius;

    [SerializeField]
    private float angleDelta;

    [SerializeField]
    private float radiusToLengthFactor;

    private int numberOfActiveCylinderParts;

	public void Start() {
        Assert.IsNotNull(CylinderPrefab, "CylinderTemplate must be initialized");
        numberOfActiveCylinderParts = NumberOfCylinders;
        cylinders = new Transform[NumberOfCylinders];

        angleDelta = 360.0f / NumberOfCylinders;

        radiusToLengthFactor = Mathf.Sin(Mathf.Deg2Rad * angleDelta / 2) / Mathf.Cos(Mathf.Deg2Rad * angleDelta / 2);

        // Initialize the cylinders that make the wave shape:
        for (int i = 0; i < NumberOfCylinders; i++) {
            Transform cylinder = Object.Instantiate(CylinderPrefab);
            cylinder.gameObject.layer = this.gameObject.layer;
            cylinder.gameObject.GetComponent<MeshRenderer>().sharedMaterial = WaveMaterial;
            cylinder.parent = this.transform;

            // Must also reset transform here, even if the same is done in OnEnable (or the rotation will get wrong):
            cylinder.localPosition = Vector3.zero;
            cylinder.localEulerAngles = new Vector3(90, -i * angleDelta, 0);
            cylinder.localScale = new Vector3(CylinderDiameter, 0, CylinderDiameter);

            cylinders[i] = cylinder;
        }
    }

    public void OnEnable() {
        CurrentWaveRadius = 0;

        if (cylinders == null) {
            // This method was called before Start. Do nothing.
            return;
        }

        numberOfActiveCylinderParts = NumberOfCylinders;

        // Reset all transforms:
        for (int i = 0; i < NumberOfCylinders; i++) {
            Transform cylinder = cylinders[i];
            cylinder.localPosition = Vector3.zero;
            cylinder.localEulerAngles = new Vector3(90, -i * angleDelta, 0);
            cylinder.localScale = new Vector3(CylinderDiameter, 0, CylinderDiameter);
            cylinder.gameObject.SetActive(true);
        }
	}
	
    public void FixedUpdate() {
        CurrentWaveRadius += RadiusExpansionRate * Time.fixedDeltaTime;

        for (int i = 0; i < NumberOfCylinders; i++) {
            Transform cylinder = cylinders[i];
            cylinder.localPosition = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angleDelta * i) * CurrentWaveRadius, 0, Mathf.Sin(Mathf.Deg2Rad * angleDelta * i) * CurrentWaveRadius);
            float extraLengthToCloseOuterGap = Mathf.Sin(Mathf.Deg2Rad * angleDelta / 2.0f) * (CylinderDiameter/2.0f);
            cylinder.localScale = new Vector3(CylinderDiameter, CurrentWaveRadius * radiusToLengthFactor + extraLengthToCloseOuterGap, CylinderDiameter);
        }
    }

    public void OnCylinderLeftPlayfield()
    {
        Debug.Log("Child WaveCylinder left playfield.");
        numberOfActiveCylinderParts--;

        if (numberOfActiveCylinderParts <= 0) {
            // Fade out the cylinder:
            DOTween.To(getRadius, setRadius, CurrentWaveRadius + 20, 1)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }

    private float getRadius() {
        return CurrentWaveRadius;
    }

    private void setRadius(float radius) {
        CurrentWaveRadius = radius;
    }
}
