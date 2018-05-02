using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDataGenerator : MonoBehaviour {
    public int width = 256;
    public int length = 256;
    [Range(20.0f, 40.0f)]
    public float bump;
    [Range(0.0f, 100.0f)]
    public float scale;
    [Range(0.0f, 255.0f)]
    public float offsetX = 100;
    [Range(0.0f, 255.0f)]
    public int offsetY = 100;

    Terrain terrain;
	// Use this for initialization
	void Start () { 
        
	}
	
	// Update is called once per frame
	void Update () {
        terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrainData(terrain.terrainData);
	}

    TerrainData GenerateTerrainData(TerrainData terrainData)
    {
        
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, bump, length);
        terrainData.SetHeights(0, 0, GenerateHeightmap());
        return terrainData;
    }

    float[,] GenerateHeightmap()
    {
        float[,] heightMap = new float[width, length];
        for(int x = 0; x < width; ++x)
        {
            for(int z = 0; z < length; ++z)
            {
                heightMap[x,z] = CalculateHeight(x, z);
            }
        }
        return heightMap;
    }

    float CalculateHeight(int x, int z)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)z / length * scale + offsetY;

        return Perlin.Noise(xCoord, yCoord, 0);
    }
}
