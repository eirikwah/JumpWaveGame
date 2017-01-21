using UnityEngine;
using System;

public class WaveCreator : MonoBehaviour {
    public GameObject[] Waves;

    /*public void Start() {
        Invoke("CreateWaveTest", 3);
        Invoke("CreateWaveTest", 4);
    }

    public void CreateWaveTest() {
        CreateWave(new Vector3(3,3,3), "Player2");
    }*/

    /// <summary>
    /// Creates a new wave.
    /// </summary>
    /// <param name="globalPosition">Global position.</param>
    /// <param name="playerName">Player name.</param>
    public void CreateWave(Vector3 globalPosition, String playerName) {
        Debug.Log("Creating wave for " + playerName);

        // E.g. "Player1" for first player
        int waveIndex = int.Parse(playerName.Substring(6)) -1;
        GameObject wave = Waves[waveIndex]; //Instantiate(Resources.Load<GameObject>("Wave"));

        if (wave.activeInHierarchy)
        {
            Debug.Log("Wave for " + playerName + " (index " + waveIndex + ") is already active!");
            return;
        }

        wave.transform.position = new Vector3(globalPosition.x, 0, globalPosition.z);
        wave.SetActive(true);
    }
	
}
