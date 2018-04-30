using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator{
    public List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> normals = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    public List<int> triangleIndexs = new List<int>();
    private Mesh mesh;

    public void BuildTriangle(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0);
        BuildTriangle(v0, v1, v2, normal);
    }

    public void BuildTriangle(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 normal)
    {
        int vertex0Index = vertices.Count;
        int vertex1Index = vertices.Count + 1;
        int vertex2Index = vertices.Count + 2;

        vertices.Add(v0);
        vertices.Add(v1);
        vertices.Add(v2);

        normals.Add(normal);
        normals.Add(normal);
        normals.Add(normal);
  

        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        

        triangleIndexs.Add(vertex0Index);
        triangleIndexs.Add(vertex1Index);
        triangleIndexs.Add(vertex2Index);
    }

    public Mesh CreateMesh()
    {
        if (mesh == null)
            mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangleIndexs.ToArray();

        return mesh;
    }

    public void Clear()
    {
        if (mesh != null)
            mesh.Clear();
        vertices.Clear();
        normals.Clear();
        uvs.Clear();
        triangleIndexs.Clear();
    }

    public void addVertices(Vector3 v)
    {
        vertices.Add(v);
    }

    public void BuildQuad(int a, int b, int c, int d)
    {
        triangleIndexs.Add(b);
        triangleIndexs.Add(c);
        triangleIndexs.Add(a);
        //triangleIndexs.Add(b, c, d);
        triangleIndexs.Add(b);
        triangleIndexs.Add(d);
        triangleIndexs.Add(c);
    }
}
