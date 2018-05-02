using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TerrainMaker : MonoBehaviour {
    private List<Vector3> tmpVertices = new List<Vector3>();
    Mesh mesh;
    [Range(1.0f, 6.0f)]
    public float xSquareSize = 1.0f;
    [Range(1.0f, 6.0f)]
    public float zSquareSize = 1.0f;
    public AnimationCurve animationCurve;

    Color[] colorMap;
    private float xSize;
    private float zSize;
    public int xCount = 50;
    public int zCount = 50;
    [Range(10.0f, 40.0f)]
    public float bump = 20.0f;
    [Range(0f, 1.0f)]
    public float rangeValue;
    [Range(0f, 255.0f)]
    public float perlinXvalue;
    [Range(0f, 255.0f)]
    public float perlinYvalue;
    int[] triangles;
    [Range(10f, 20.0f)]
    public float frequency;
    Vector3[] vertices;
    Vector3[] normals;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Vector2[] uvs;
	// Use this for initialization
	void Start () {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
       
	}
	
	// Update is called once per frame
	void Update () {
        xSize = xCount * xSquareSize;
        zSize = zCount * zSquareSize;
        vertices = new Vector3[(xCount + 1) * (zCount + 1)];
        normals = new Vector3[(xCount + 1) * (zCount + 1)];
        uvs = new Vector2[(xCount + 1) * (zCount + 1)];
        colorMap = new Color[(xCount + 1) * (zCount + 1)];
        triangles = new int[xCount * zCount * 6];
        for (int z = 0, i = 0; z <= zCount; ++z)
        {
            for (int x = 0; x <= xCount; ++x, ++i)
            {
                float noiseValue = (Perlin.Noise((float)x * frequency / xCount + perlinXvalue, (float)z  * frequency/ zCount + perlinYvalue, 0) + 1.0f) / 2.0f;
                //Debug.Log(noiseValue);
                //float noiseValue = Perlin.Noise((float)x / xCount, (float)z / zCount, 0) + 0.5f * Perlin.Noise((float) 2 *x / xCount, (float)z * 2/ zCount, 0)
                //    +0.25f * Perlin.Noise((float) x * 4 / xCount, (float)z * 4/ zCount, 0);
                if (noiseValue <= 0.4f)
                    colorMap[z * xCount + x] = Color.blue;
                else if (noiseValue > 0.4f && noiseValue <= 0.6f)
                    colorMap[z * xCount + x] = Color.green;
                else if (noiseValue > 0.6f && noiseValue <= 1.0F)
                    colorMap[z * xCount + x] = Color.red;

                uvs[i] = new Vector2((float)x / xCount, (float)z / zCount);
                vertices[i] = new Vector3(x * xSquareSize,animationCurve.Evaluate(noiseValue) * bump, z * zSquareSize);
                
            }
        }
        mesh.vertices = vertices;
        for (int z = 0, i = 0, t = 0; z < zCount; ++z, ++i)   
        {
            for(int x =0; x < xCount; ++x, ++i, t+=6)
            {
                triangles[t] = i;
                triangles[t + 1] = i + xCount + 1;
                triangles[t + 2] = i + 1;
                triangles[t + 3] = i + 1;
                triangles[t + 4] = i + xCount + 1;
                triangles[t + 5] = i + xCount + 2;
            }
        }

        for(int i = 0; i < vertices.Length; ++i)
        {
           
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uvs;
        meshFilter.mesh = mesh;
        meshRenderer.material.mainTexture = TextureGenerator.Texture2DFromColorMap(colorMap, xCount, zCount);
	}

}
