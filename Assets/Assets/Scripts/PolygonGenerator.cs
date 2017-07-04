using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour
{
    // Contains every vertex of the mesh that'll be rendered
    public List<Vector3> newVertices = new List<Vector3>();

    // Indexes for triangles
    public List<int> newTriangles = new List<int>();

    // The UV coordinates for the mesh
    public List<Vector2> newUV = new List<Vector2>();

    // The mesh being created(?)
    Mesh mesh;

    // Use this for initialization
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;


        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x, y - 1, z));

        newTriangles.Add(0);
        newTriangles.Add(1);
        newTriangles.Add(3);
        newTriangles.Add(1);
        newTriangles.Add(2);
        newTriangles.Add(3);

        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        // mesh.Optimize(); no longer supported apparently
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
