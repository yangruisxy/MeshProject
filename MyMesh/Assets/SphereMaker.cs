using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class SphereMaker : MonoBehaviour {
    public float radius = 1.0f;
    public int resolution = 5;
    MeshGenerator mg = new MeshGenerator();
    MeshFilter meshFilter;
    private List<Vector3> nvertices = new List<Vector3>();
    float var = 0.0f;

    // use origin right up to represent a face
    static Vector3[] baseOrigin =
    {
        new Vector3(-1.0f, -1.0f, -1.0f),
        new Vector3(1.0f, -1.0f, -1.0f),
        new Vector3(1.0f, -1.0f, 1.0f),
        new Vector3(-1.0f, -1.0f, 1.0f),
        new Vector3(-1.0f, 1.0f, -1.0f),
        new Vector3(-1.0f, -1.0f, 1.0f)
    };

    static Vector3[] constRight =
    {
        new Vector3(2.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 2.0f),
        new Vector3(-2.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, -2.0f),
        new Vector3(2.0f, 0.0f, 0.0f),
        new Vector3(2.0f, 0.0f, 0.0f)
    };

    static Vector3[] constUp =
    {
        new Vector3(0.0f, 2.0f, 0.0f),
        new Vector3(0.0f, 2.0f, 0.0f),
        new Vector3(0.0f, 2.0f, 0.0f),
        new Vector3(0.0f, 2.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 2.0f),
        new Vector3(0.0f, 0.0f, -2.0f)
    };
	// Use this for initialization
	void Start () {
        meshFilter = this.GetComponent<MeshFilter>();	
	}
	
	// Update is called once per frame
	void Update () {
        mg.Clear();
        var += Random.Range(0.05f, 0.2f);
        CreateSphere(resolution);
	}

    private void CreateSphere(int resolution)
    {
        float step = 1.0f / resolution;
       
        //Vector3 step3 = new Vector3(step, step, step);
        for (int face = 0; face < 6; ++face)
        {
            
            Vector3 origin = baseOrigin[face];
            Vector3 right = constRight[face];
            Vector3 up = constUp[face];
            for (int j = 0; j < resolution + 1; ++j)
            {
                for (int i = 0; i < resolution + 1; ++i)
                {
                    
                    Vector3 p = origin + step * (i * right + j * up);
                    //float noiseValue = PerlinNoise.Noise(MappingFunction(0, 100, (int)p.x * 5), MappingFunction(0, 100, (int)p.z * 5), 0);
                    //Debug.Log(noiseValue);
                    Vector3 p2 = new Vector3(p.x * p.x, p.y * p.y, p.z * p.z);
                    Vector3 n = new Vector3(p.x *Mathf.Sqrt(1.0f - 0.5f * (p2.y + p2.z) + p2.y * p2.z / 3.0f),
                        p.y * Mathf.Sqrt(1.0f - 0.5f * (p2.x + p2.z) + p2.x * p2.z / 3.0f),
                        p.z * Mathf.Sqrt(1.0f - 0.5f * (p2.y + p2.x) + p2.x * p2.y / 3.0f)) * 5f;
                    //float noiseValue = PerlinNoise.Noise(Random.Range(0,100), Random.Range(0, 100), 0);
                    //Debug.Log(noiseValue);
                    
                    nvertices.Add(n);
                }
            }
        }

        //for (int i = 0; i < nvertices.Count; ++i)
        //{
        //    float noiseValue = PerlinNoise.Noise(MappingFunction(0, 100, i), Random.Range(0, 100), 0);
        //    nvertices[i] += new Vector3(0, 0.9f * noiseValue, 0);
        //}
        int k = resolution + 1;
        Debug.Log(nvertices[0].y);
        for (int face = 0; face < 6; ++face)
        {
            for (int j = 0; j < resolution; ++j)
            {
                bool bottom = j < (resolution / 2);
                for (int i = 0; i < resolution; ++i)
                {
                    bool left = i < (resolution / 2);
                    int a = (face * k + j) * k + i;
                    int b = (face * k + j) * k + i + 1;
                    int c = (face * k + j + 1) * k + i;
                    int d = (face * k + j + 1) * k + i + 1;
                    mg.BuildTriangle(nvertices[b], nvertices[c], nvertices[a]);
                    mg.BuildTriangle(nvertices[b], nvertices[d], nvertices[c]);
                }
                //    }
            }
        }
        //    Vector3 a = new Vector3(6f, 6f, 6f);
        //Vector3 b = new Vector3(6f, 7f, 7f);
        //Vector3 c = new Vector3(7f, 6f, 6f);
        //Vector3 d = new Vector3(7f, 7f, 8f);

        //mg.BuildTriangle(b, c, a);
        //mg.BuildTriangle(b, d, c);

        meshFilter.mesh = mg.CreateMesh();

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        //Debug.Log(mg.vertices.Count);
        
        for(int i = 0; i < mg.vertices.Count; ++i)
        {
            Gizmos.DrawSphere(mg.vertices[i] * 10, 0.1f);
        }
    }

    private float MappingFunction(float outStart, float outEnd, int x)
    {
        return outStart + (outEnd - outStart) / 256 * x;
    }
}
