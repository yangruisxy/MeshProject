using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CubeMaker : MonoBehaviour {
    public Vector3 cubeSize = Vector3.one;
    public AudioSource audioSource;




    Vector3 halfSize;
    MeshGenerator mg = new MeshGenerator();
    Vector3 center = Vector3.zero;
    float var = 0.0f;

    private void Awake()
    {

    }
    private void Start()
    {
        halfSize = cubeSize / 2;
    }

    private void Update()
    {

        var += Random.Range(0.05f, 0.2f);
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        mg.Clear();
        int cnt = 0;
        for(int i = 0; i < 20; ++i)
            for(int j = 0; j < 20; ++j)
            { 
                float noiseValue = PerlinNoise.Noise(mapFunction(0, 100, j * cubeSize.x * 1.2f), mapFunction(0, 100, i * cubeSize.z * 1.2f), var);
               // center.Set(j * cubeSize.x * 1.2f, 1 + 0.9f * noiseValue, i * cubeSize.z * 1.2f);
                center.Set(j * cubeSize.x * 1.2f, 1 + AudioAnalysis._freqBand[j], i * cubeSize.z * 1.2f);
                CreateCube(center);
            }
        meshFilter.mesh = mg.CreateMesh();
    }

    private void CreateCube(Vector3 center)
    {
        Vector3 t0 = new Vector3(center.x + halfSize.x, center.y + halfSize.y, center.z + halfSize.z);
        Vector3 t1 = new Vector3(center.x + halfSize.x, center.y + halfSize.y, center.z - halfSize.z);
        Vector3 t2 = new Vector3(center.x - halfSize.x, center.y + halfSize.y, center.z - halfSize.z);
        Vector3 t3 = new Vector3(center.x - halfSize.x, center.y + halfSize.y, center.z + halfSize.z);

        Vector3 b0 = new Vector3(center.x + halfSize.x, center.y - halfSize.y, center.z + halfSize.z);
        Vector3 b1 = new Vector3(center.x - halfSize.x, center.y - halfSize.y, center.z + halfSize.z);
        Vector3 b2 = new Vector3(center.x - halfSize.x, center.y - halfSize.y, center.z - halfSize.z);
        Vector3 b3 = new Vector3(center.x + halfSize.x, center.y - halfSize.y, center.z - halfSize.z);

        //top square
        mg.BuildTriangle(t0, t1, t2);
        mg.BuildTriangle(t0, t2, t3);

        //bottom square
        mg.BuildTriangle(b0, b1, b2);
        mg.BuildTriangle(b0, b2, b3);

        //front square
        mg.BuildTriangle(t1, b3, b2);
        mg.BuildTriangle(t1, b2, t2);

        //back square
        mg.BuildTriangle(t0, t3, b1);
        mg.BuildTriangle(t0, b1, b0);

        //left square
        mg.BuildTriangle(t3, t2, b2);
        mg.BuildTriangle(t3, b2, b1);

        //right square
        mg.BuildTriangle(t1, t0, b0);
        mg.BuildTriangle(t1, b0, b3);
    }

    private float mapFunction(int outStart, int outEnd, float x)
    {
        return outStart + (outEnd - outStart) / 23.8f * x;
    }
}
