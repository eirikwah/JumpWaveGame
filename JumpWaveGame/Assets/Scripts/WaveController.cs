using UnityEngine;
using UnityEngine.Assertions;

public class WaveController : MonoBehaviour {
    public Transform CylinderPrefab; 
    public Material WaveMaterial;

    public float CylinderDiameter = 1;
    public int NumberOfCylinders = 16;
    public float RadiusToLengthFactor = 0.2f;
    public float RadiusExpansionRate = 3;

    private Transform[] cylinders; 

    [SerializeField]
    private float CurrentWaveRadius;

    [SerializeField]
    private float angleDelta;

	public void Start () {
        Assert.IsNotNull(CylinderPrefab, "CylinderTemplate must be initialized");
        cylinders = new Transform[NumberOfCylinders];

        angleDelta = 360.0f / NumberOfCylinders;

        // Reset all positions:
        for (int i = 0; i < NumberOfCylinders; i++) {
            Transform cylinder = Object.Instantiate(CylinderPrefab);
            cylinder.gameObject.GetComponent<MeshRenderer>().sharedMaterial = WaveMaterial;
            cylinder.parent = this.transform;
            cylinder.localPosition = Vector3.zero;
            cylinder.localEulerAngles = new Vector3(90, -i * angleDelta, 0);
            cylinder.localScale = new Vector3(CylinderDiameter, 0, CylinderDiameter);
            cylinders[i] = cylinder;
        }
	}
	
    public void FixedUpdate () {
        CurrentWaveRadius += RadiusExpansionRate * Time.fixedDeltaTime;

        for (int i = 0; i < NumberOfCylinders; i++) {
            Transform cylinder = cylinders[i];
            cylinder.localPosition = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angleDelta * i) * CurrentWaveRadius, 0, Mathf.Sin(Mathf.Deg2Rad * angleDelta * i) * CurrentWaveRadius);
            cylinder.localScale = new Vector3(CylinderDiameter, CurrentWaveRadius * RadiusToLengthFactor + Mathf.Sin(Mathf.Deg2Rad * angleDelta / 2.0f) * (CylinderDiameter/2.0f), CylinderDiameter);
        }

    }


}
