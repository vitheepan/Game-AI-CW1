using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public GameObject goldcoinPrefab;

    public GameObject playerPrefab;
    public GameObject botOnePrefabs;
    public GameObject botTwoPrefabs;

    public int numberOfCoins = 10;
    public int numberOfHouse = 10;
    public int numberOfRocks = 10;
    public int numberOfTrees = 10;
    public float minDistance = 10f;
    public float maxDistance = 20f;

    private Mesh mesh;
    private new Renderer renderer;
    public Vector3[] vertices;
    private int[] triangles;

    NavMeshSurface navMeshSurface;

    private void Start()
    {
        mesh = new Mesh();
        renderer = GetComponent<Renderer>();
        GetComponent<MeshFilter>().mesh = mesh;
        navMeshSurface = GetComponent<NavMeshSurface>();
        


        GenerateTerrain();
        UpdateMesh();

        SpawnTrees();
        SpawnRocks();
        SpawnHouses();
        SpawnCoins();

        Vector3 playerSpawnPoint = new Vector3(Random.Range(0f, 1f) * width, 0f, Random.Range(0f, 1f) * height);
        Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);

        Vector3 botOneSpawnPoint = new Vector3(Random.Range(0f, 1f) * width, 0f, Random.Range(0f, 1f) * height);
        Instantiate(botOnePrefabs, botOneSpawnPoint, Quaternion.identity);

        Vector3 botTwoSpawnPoint = new Vector3(Random.Range(0f, 1f) * width, 0f, Random.Range(0f, 1f) * height);
        Instantiate(botTwoPrefabs, botTwoSpawnPoint, Quaternion.identity);

    }

    private void GenerateTerrain()
    {
        float xOffset = -transform.position.x;
        float yOffset = -transform.position.z;

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
        navMeshSurface.BuildNavMesh();
        

    }

    private void SpawnTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            float randomX = Random.Range(0f, width);
            float randomY = Random.Range(0f, height);
            Vector3 randomPosition = new Vector3(randomX, 0f, randomY);

            bool tooClose = false;
            foreach (GameObject tree in GameObject.FindGameObjectsWithTag("Tree"))
            {
                if (Vector3.Distance(tree.transform.position, randomPosition) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

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
            float randomX = Random.Range(0f, width);
            float randomY = Random.Range(0f, height);
            Vector3 randomPosition = new Vector3(randomX, 0f, randomY);

            bool tooClose = false;
            foreach (GameObject rock in GameObject.FindGameObjectsWithTag("Rock"))
            {
                if (Vector3.Distance(rock.transform.position, randomPosition) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

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
            float randomX = Random.Range(0f, width);
            float randomY = Random.Range(0f, height);
            Vector3 randomPosition = new Vector3(randomX, 0f, randomY);

            bool tooClose = false;
            foreach (GameObject house in GameObject.FindGameObjectsWithTag("House"))
            {
                if (Vector3.Distance(house.transform.position, randomPosition) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                Vector3 housePosition = new Vector3(randomX, SampleTerrain(randomX, randomY), randomY);
                Instantiate(housePrefab, housePosition, Quaternion.identity);
            }
        }
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            float randomX = Random.Range(0f, width);
            float randomY = Random.Range(0f, height);
            Vector3 randomPosition = new Vector3(randomX, 0f, randomY);

            bool tooClose = false;
            foreach (GameObject coin in GameObject.FindGameObjectsWithTag("Coin"))
            {
                if (Vector3.Distance(coin.transform.position, randomPosition) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                Vector3 coinPosition = new Vector3(randomX, SampleTerrain(randomX, randomY), randomY);
                Instantiate(goldcoinPrefab, coinPosition, Quaternion.identity);
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
