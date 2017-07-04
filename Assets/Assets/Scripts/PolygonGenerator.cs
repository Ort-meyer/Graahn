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

        GenSquare((int)x, (int)y, tGrass);

        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* 
    This method updates the entire mesh with new vertices and triangles
    Captain's log, 4/7 19:29
    It's always weird to continually update meshes. We do it with the spring water in project Frog
    as well. Isn't it a huge performance cost? Send the new mesh to the gpu. Maybe unity optimizes 
    it? Maybe it's not called as often here and that can be applied to Frog? We'll see
    End captain's log*/
    void UpdateMesh()
    { 
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.RecalculateNormals();
    }


    // Generates a square at the given coordinates with the specified texture coordinate
    void GenSquare(int x, int y, Vector2 tCoord)
    {
        // Hard coded -5 in z so far
        newVertices.Add(new Vector3(x, y, -5));
        newVertices.Add(new Vector3(x + 1, y, -5));
        newVertices.Add(new Vector3(x + 1, y - 1, -5));
        newVertices.Add(new Vector3(x, y - 1, -5));

        newTriangles.Add(0);
        newTriangles.Add(1);
        newTriangles.Add(3);
        newTriangles.Add(1);
        newTriangles.Add(2);
        newTriangles.Add(3);

        newUV.Add(new Vector2(tUnit * tCoord.x, tUnit * tCoord.y + tUnit));
        newUV.Add(new Vector2(tUnit * tCoord.x + tUnit, tUnit * tCoord.y + tUnit));
        newUV.Add(new Vector2(tUnit * tCoord.x + tUnit, tUnit * tCoord.y));
        newUV.Add(new Vector2(tUnit * tCoord.x, tUnit * tCoord.y));
    }
}
