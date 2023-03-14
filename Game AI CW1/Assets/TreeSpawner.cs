using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab;
    public int numberOfTrees;
    public float spawnRadius;

    private Vector3 centerPos;


    private void Start()
    {
        // Get center position
        centerPos = transform.position;

        for (int i = 0; i < numberOfTrees; i++)
        {
            // Generate random position around center
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 randomPosition = new Vector3(randomCircle.x, 0, randomCircle.y) + centerPos;

            // Spawn item at random position
            Instantiate(treePrefab, randomPosition, Quaternion.identity);

        }
    }
}