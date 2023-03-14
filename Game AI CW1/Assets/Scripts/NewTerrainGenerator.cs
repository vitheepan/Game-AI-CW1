using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTerrainGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20f;

    public Color[] colors;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    private Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        float[,] noiseMap = Noise.GenerateNoiseMap(width, height, scale);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = colors[0];

                for (int i = 0; i < colors.Length - 1; i++)
                {
                    if (noiseMap[x, y] < (i + 1f) / (colors.Length - 1))
                    {
                        color = Color.Lerp(colors[i], colors[i + 1], noiseMap[x, y] * (colors.Length - 1) - i);
                        break;
                    }
                }

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();

        return texture;
    }
}
