using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public int numItemsToSpawn = 10;
    public float spawnRange = 10f;
    public GameObject terrain;

    void Start()
    {
        for (int i = 0; i < numItemsToSpawn; i++)
        {
            // Generate a random position within the defined spawn range
            float x = Random.Range(-spawnRange, spawnRange);
            float z = Random.Range(-spawnRange, spawnRange);
            Vector3 spawnPosition = new Vector3(x, 0f, z);

            // Get the height of the terrain at the random position
            //float height = terrain.SampleHeight(spawnPosition);

            // Set the Y position of the spawn position to the terrain height
            //spawnPosition.y = height;

            // Instantiate the item prefab at the randomly generated position
            GameObject item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            item.transform.parent = transform;
        }
    }
}
