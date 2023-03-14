using System.Collections.Generic;
using UnityEngine;

public class PerlinTerrainGenerator : MonoBehaviour
{
    public int size = 50; // This is the number of vertices on each axis
    public float scale = 10.0f; // This is the scale of the terrain
    public float heightMultiplier = 5.0f; // The multiplier for the height of the terrain
    public Gradient gradient; // The color gradient for the terrain

    public GameObject itemPrefab; // The prefab of the item to be spawned
    public int numItems = 10; // The number of items to be spawned

    private float[,] nMap;

    public void Start()
    {

        transform.Rotate(new Vector3(0f, 180f, 0f));

        // Create an empty mesh
        Mesh mesh = new Mesh();

        // Generate the noise map
        float[,] noiseMap = GenerateNoiseMap(size, scale);

        // Create a list of vertices, colors, and triangles
        List<Vector3> vertices = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> triangles = new List<int>();

        // Loop through each point on the noise map and create a vertex
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                // Calculate the height of the vertex based on the noise map
                float height = -noiseMap[x, z] * heightMultiplier;

                // Create the vertex
                Vector3 vertex = new Vector3(x, height, z);
                vertices.Add(vertex);

                // Calculate the color of the vertex based on the gradient
                Color color = gradient.Evaluate(noiseMap[x, z]);
                colors.Add(color);

                // Create triangles for the mesh
                if (x < size - 1 && z < size - 1)
                {
                    int vertexIndex = x * size + z;

                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + size);
                    triangles.Add(vertexIndex + 1);

                    triangles.Add(vertexIndex + 1);
                    triangles.Add(vertexIndex + size);
                    triangles.Add(vertexIndex + size + 1);
                }
            }
        }

        // Set the vertices, colors, and triangles for the mesh
        mesh.vertices = vertices.ToArray();
        mesh.colors = colors.ToArray();
        mesh.triangles = triangles.ToArray();

        // Calculate the mesh normals
        mesh.RecalculateNormals();

        // Set the mesh for the MeshFilter component
        GetComponent<MeshFilter>().mesh = mesh;

        SpawnItems();
    }

    // Generate a 2D Perlin noise map
    float[,] GenerateNoiseMap(int size, float scale)
    {
        float[,] noiseMap = new float[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                float xCoord = (float)x / size * scale;
                float zCoord = (float)z / size * scale;

                // Generate Perlin noise at the given coordinates
                float sample = Mathf.PerlinNoise(xCoord, zCoord);

                noiseMap[x, z] = sample;
            }
        }
        nMap = noiseMap;
        return noiseMap;
    }


    void SpawnItems()
    {
        // Loop through the number of items to be spawned
        for (int i = 0; i < numItems; i++)
        {
            // Generate a random position on the terrain
            float x = Random.Range(0, size);
            float z = Random.Range(0, size);
            float y = nMap[(int)x, (int)z] * heightMultiplier;

            // Instantiate the item prefab at the random position
            GameObject item = Instantiate(itemPrefab, new Vector3(x, y, z), Quaternion.identity);

            // Set the item's parent to the PerlinTerrainGenerator object
            item.transform.parent = transform;
        }
    }

}
