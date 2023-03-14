using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinGeneration : MonoBehaviour
{
    public int width = 64;
    public int height = 64;
    public float depth = 20f;
    public float scale = 20f;
    public float xOffset = 100f;
    public float yOffset = 100f;
    public float amplitude = 10f;
    public float frequency = 1f;
    public Gradient gradient;

    public GameObject treePrefab;
    public GameObject rockPrefab;
    public GameObject housePrefab;

    public int numberOfHouse = 10;
    public int numberOfRocks = 10;
    public int numberOfTrees = 10;
    public float minDistance = 10f;
    public float maxDistance = 20f;

    private Mesh mesh;
    private new Renderer renderer;
    public Vector3[] vertices;
    private int[] triangles;

    private void Start()
    {
        mesh = new Mesh();
        renderer = GetComponent<Renderer>();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateTerrain();
        UpdateMesh();

        SpawnTrees();
        SpawnRocks();
        SpawnHouses();
    }

    private void GenerateTerrain()
    {
        vertices = new Vector3[(width + 1) * (height + 1)];

        for (int y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
                float xCoord = (float)x / width * scale + xOffset;
                float yCoord = (float)y / height * scale + yOffset;

                float sample = Mathf.PerlinNoise(xCoord * frequency, yCoord * frequency) * amplitude;

                vertices[y * (width + 1) + x] = new Vector3(x, sample, y);
            }
        }

        triangles = new int[width * height * 6];

        int vertIndex = 0;
        int triIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                triangles[triIndex] = vertIndex;
                triangles[triIndex + 1] = vertIndex + width + 1;
                triangles[triIndex + 2] = vertIndex + 1;
                triangles[triIndex + 3] = vertIndex + 1;
                triangles[triIndex + 4] = vertIndex + width + 1;
                triangles[triIndex + 5] = vertIndex + width + 2;

                vertIndex++;
                triIndex += 6;
            }

            vertIndex++;
        }
    }

    private void UpdateMesh()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        Vector2[] uv = new Vector2[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            uv[i] = new Vector2(vertices[i].x / width, vertices[i].z / height);
        }

        mesh.uv = uv;

        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = gradient.Evaluate(vertices[i].y / depth);
        }

        mesh.colors = colors;
    }

    private void SpawnTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            // Generate a random position within the terrain bounds
            float randomX = Random.Range(0f, width);
            float randomY = Random.Range(0f, height);
            Vector3 randomPosition = new Vector3(randomX, 0f, randomY);

            // Check if the position is too close to an existing tree
            bool tooClose = false;
            foreach (GameObject tree in GameObject.FindGameObjectsWithTag("Tree"))
            {
                if (Vector3.Distance(tree.transform.position, randomPosition) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            // Spawn a tree prefab if the position is valid
            if (!tooClose)
            {
                Vector3 treePosition = new Vector3(randomX, SampleTerrain(randomX, randomY), randomY);
                Instantiate(treePrefab, treePosition, Quaternion.identity);
            }
        }
    }

    private void SpawnRocks()
    {
        for (int i = 0; i < numberOfRocks; i++)
        {
            // Generate a random position within the terrain bounds
            float randomX = Random.Range(0f, width);
            float randomY = Random.Range(0f, height);
            Vector3 randomPosition = new Vector3(randomX, 0f, randomY);

            // Check if the position is too close to an existing tree
            bool tooClose = false;
            foreach (GameObject rock in GameObject.FindGameObjectsWithTag("Rock"))
            {
                if (Vector3.Distance(rock.transform.position, randomPosition) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            // Spawn a tree prefab if the position is valid
            if (!tooClose)
            {
                Vector3 rockPosition = new Vector3(randomX, SampleTerrain(randomX, randomY), randomY);
                Instantiate(rockPrefab, rockPosition, Quaternion.identity);
            }
        }
    }

    private void SpawnHouses()
    {
        for (int i = 0; i < numberOfHouse; i++)
        {
            // Generate a random position within the terrain bounds
            float randomX = Random.Range(0f, width);
            float randomY = Random.Range(0f, height);
            Vector3 randomPosition = new Vector3(randomX, 0f, randomY);

            // Check if the position is too close to an existing tree
            bool tooClose = false;
            foreach (GameObject house in GameObject.FindGameObjectsWithTag("House"))
            {
                if (Vector3.Distance(house.transform.position, randomPosition) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            // Spawn a tree prefab if the position is valid
            if (!tooClose)
            {
                Vector3 housePosition = new Vector3(randomX, SampleTerrain(randomX, randomY), randomY);
                Instantiate(housePrefab, housePosition, Quaternion.identity);
            }
        }
    }

    private float SampleTerrain(float x, float y)
    {
        float xCoord = x / width * scale + xOffset;
        float yCoord = y / height * scale + yOffset;
        float sample = Mathf.PerlinNoise(xCoord * frequency, yCoord * frequency) * amplitude;
        return sample;
    }

}
