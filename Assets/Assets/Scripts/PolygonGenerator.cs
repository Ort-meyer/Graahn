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
    
    // The fraction of space that 1 tile takes up of the entire texture (there's 4 textures now, so 1/4)
    private float tUnit = 0.25f;
    // Coordinates for text textures
    private Vector2 tStone = new Vector2(0, 0);
    private Vector2 tGrass = new Vector2(0, 1);

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

        Vector2 t = tGrass;

        newUV.Add(new Vector2(tUnit * t.x, tUnit * t.y + tUnit));
        newUV.Add(new Vector2(tUnit * t.x + tUnit, tUnit * t.y + tUnit));
        newUV.Add(new Vector2(tUnit * t.x + tUnit, tUnit * t.y));
        newUV.Add(new Vector2(tUnit * t.x, tUnit * t.y));

        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        // mesh.Optimize(); no longer supported apparently
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
