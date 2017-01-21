using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlacer : MonoBehaviour
{
	public int sizeX;
	public int sizeY;
	public float tileOffset;
	public List<GameObject> prefab;

	public void Awake()
	{
		//prefab = new List<GameObject>();
	}

	public void Start()
	{
		GameObject current;

		for(int x = 0; x < sizeX; x++)
		{
			for(int y = 0; y < sizeY; y++)
			{
				GameObject thisPrefab = prefab[Random.Range(0, prefab.Count-1)];
				current = Instantiate (thisPrefab, new Vector3(gameObject.transform.position.x + x * tileOffset, gameObject.transform.position.y, gameObject.transform.position.z + y * tileOffset), thisPrefab.transform.rotation);
				current.transform.SetParent (transform.root);
			}
		}	
	}
}
