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

    }

    // Update is called once per frame
    void Update()
    {

    }
}
