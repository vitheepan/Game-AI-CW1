using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectSpawner : MonoBehaviour
{

    public GameObject objectToSpawn;
    public int numberOfObjects;
    public float spawnRadius;

    void Start()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, 1);
        return hit.position;
    }
}
