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

    // Array for the blocks we will create. This stores the block types. 0 is air, 1 is rock and 2 is grass. Needs to be expanded later...
    public byte[,] blocks;

    // The mesh being created(?)
    Mesh mesh;

    // The fraction of space that 1 tile takes up of the entire texture (there's 4 textures now, so 1/4)
    private float tUnit = 0.25f;
    // Coordinates for text textures
    private Vector2 tStone = new Vector2(0, 0);
    private Vector2 tGrass = new Vector2(0, 1);

    // Counts the number of squares we have. Used for indices when creating squares, if nothing else
    private int squareCount;

    // Use this for initialization
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        // GenSquare((int)x, (int)y, tGrass);
        GenTerrain();
        BuildMesh();
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // This method updates the entire mesh with new vertices and triangles
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.RecalculateNormals();

        squareCount = 0;
        newVertices.Clear();
        newTriangles.Clear();
        newUV.Clear();
    }


    // Generates a square at the given coordinates with the specified texture coordinate
    void GenSquare(int x, int y, Vector2 tCoord)
    {
        // Hard coded -5 in z so far
        newVertices.Add(new Vector3(x, y, -5));
        newVertices.Add(new Vector3(x + 1, y, -5));
        newVertices.Add(new Vector3(x + 1, y - 1, -5));
        newVertices.Add(new Vector3(x, y - 1, -5));

        newTriangles.Add(squareCount * 4 + 0);
        newTriangles.Add(squareCount * 4 + 1);
        newTriangles.Add(squareCount * 4 + 3);
        newTriangles.Add(squareCount * 4 + 1);
        newTriangles.Add(squareCount * 4 + 2);
        newTriangles.Add(squareCount * 4 + 3);

        newUV.Add(new Vector2(tUnit * tCoord.x, tUnit * tCoord.y + tUnit));
        newUV.Add(new Vector2(tUnit * tCoord.x + tUnit, tUnit * tCoord.y + tUnit));
        newUV.Add(new Vector2(tUnit * tCoord.x + tUnit, tUnit * tCoord.y));
        newUV.Add(new Vector2(tUnit * tCoord.x, tUnit * tCoord.y));

        squareCount++;
    }

    // Generates terrain. Half will be grass, the other will be rock. VERY hard-coded so far
    void GenTerrain()
    {
        blocks = new byte[10, 10];
        for (int px = 0; px < blocks.GetLength(0); px++)
        {
            for (int py = 0; py < blocks.GetLength(1); py++)
            {
                if (py == 5)
                {
                    blocks[px, py] = 2;
                }
                else if (py < 5)
                {
                    blocks[px, py] = 1;
                }
            }
        }
    }

    // This builds the mesh of the terrain
    void BuildMesh()
    {
        for (int px = 0; px < blocks.GetLength(0); px++)
        {
            for (int py = 0; py < blocks.GetLength(1); py++)
            {

                if (blocks[px, py] == 1)
                {
                    GenSquare(px, py, tStone);
                }
                else if (blocks[px, py] == 2)
                {
                    GenSquare(px, py, tGrass);
                }

            }
        }
    }
}
