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

    private Mesh mesh;
    private new Renderer renderer;
    private Vector3[] vertices;
    private int[] triangles;

    private void Start()
    {
        mesh = new Mesh();
        renderer = GetComponent<Renderer>();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateTerrain();
        UpdateMesh();
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
}
