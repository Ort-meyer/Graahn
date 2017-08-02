using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour
{


    /////////////// Graphic stuffs
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



    ////////////// Collision stuffs
    // Vertices for collision mesh
    public List<Vector3> colVertices = new List<Vector3>();
    // Indices for the vertics
    public List<int> colTriangles = new List<int>();
    // The amount of something???
    private int colCount;

    private MeshCollider col;



    ////////////// General stuffs
    // Array for the blocks we will create. This stores the block types. 0 is air, 1 is rock and 2 is grass. Needs to be expanded later...
    public byte[,] blocks;

    // Counts the number of squares we have. Used for indices when creating squares, if nothing else
    private int squareCount;

    // Use this for initialization
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;



        // GenSquare((int)x, (int)y, tGrass);
        //GenTerrain();
        GenDebugTerrain();
        BuildMesh();
        UpdateMesh();

    }

    void GenDebugTerrain()
    {
        int sizex = 5;
        int sizey = 5;
        blocks = new byte[sizex, sizey];

        // First we fill all
        for (int i = 0; i < sizex; i++)
        {
            for (int j = 0; j < sizey; j++)
            {
                blocks[i, j] = 1;
            }
        }

        // Create pit
        blocks[1, sizey - 1] = 0;
        blocks[2, sizey - 1] = 0;
        blocks[3, sizey - 1] = 0;

        blocks[1, sizey - 2] = 0;
        blocks[2, sizey - 2] = 0;
        blocks[3, sizey - 2] = 0;

        blocks[2, sizey - 3] = 0;


    }

    // Update is called once per frame
    void Update()
    {

    }

    // This method updates the entire mesh with new vertices and triangles. The mesh is based on the grid map of terrain types
    void UpdateMesh()
    {
        // Graphical mesh update
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.RecalculateNormals();

        squareCount = 0;
        newVertices.Clear();
        newTriangles.Clear();
        newUV.Clear();


        // Collision mesh update
        Mesh newMesh = new Mesh();
        newMesh.vertices = colVertices.ToArray();
        newMesh.triangles = colTriangles.ToArray();
        col.sharedMesh = newMesh;

        colVertices.Clear();
        colTriangles.Clear();
        colCount = 0;
    }


    // Generates a square at the given coordinates with the specified texture coordinate
    void GenSquare(int x, int y, Vector2 tCoord)
    {
        newVertices.Add(new Vector3(x, y, 0));
        newVertices.Add(new Vector3(x + 1, y, 0));
        newVertices.Add(new Vector3(x + 1, y - 1, 0));
        newVertices.Add(new Vector3(x, y - 1, 0));

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

    // Generates terrain as a grid of numbers. Half will be grass, the other will be rock. VERY hard-coded so far
    void GenTerrain()
    {
        blocks = new byte[96, 128];

        for (int px = 0; px < blocks.GetLength(0); px++)
        {
            int stone = Noise(px, 0, 80, 15, 1);
            stone += Noise(px, 0, 50, 30, 1);
            stone += Noise(px, 0, 10, 10, 1);
            stone += 75;

            print(stone);

            int dirt = Noise(px, 0, 100f, 35, 1);
            dirt += Noise(px, 100, 50, 30, 1);
            dirt += 75;


            for (int py = 0; py < blocks.GetLength(1); py++)
            {
                if (py < stone)
                {
                    blocks[px, py] = 1;

                    //The next three lines make dirt spots in random places
                    if (Noise(px, py, 12, 16, 1) > 10)
                    {
                        blocks[px, py] = 2;

                    }

                    //The next three lines remove dirt and rock to make caves in certain places
                    if (Noise(px, py * 2, 16, 14, 1) > 10)
                    { //Caves
                        blocks[px, py] = 0;

                    }

                }
                else if (py < dirt)
                {
                    blocks[px, py] = 2;
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
                //If the block is not air
                if (blocks[px, py] != 0)
                {

                    // Generate collider
                    GenCollider(px, py);

                    // Generate graphical square
                    if (blocks[px, py] == 1)
                    {
                        GenSquare(px, py, tStone);
                    }
                    else if (blocks[px, py] == 2)
                    {
                        GenSquare(px, py, tGrass);
                    }
                }// End when its not an air block

            }
        }
    }

    // Generates a collision square at the given coordinate (I think it's only straight up for now tho)
    void GenCollider(int x, int y)
    {
        //Top
        if (Block(x, y + 1) == 0)
        {
            colVertices.Add(new Vector3(x, y, 1));
            colVertices.Add(new Vector3(x + 1, y, 1));
            colVertices.Add(new Vector3(x + 1, y, 0));
            colVertices.Add(new Vector3(x, y, 0));

            ColliderTriangles();
            colCount++;
        }

        //bot
        if (Block(x, y - 1) == 0)
        {
            colVertices.Add(new Vector3(x, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y - 1, 1));
            colVertices.Add(new Vector3(x, y - 1, 1));

            ColliderTriangles();
            colCount++;
        }

        //left
        if (Block(x - 1, y) == 0)
        {
            colVertices.Add(new Vector3(x, y - 1, 1));
            colVertices.Add(new Vector3(x, y, 1));
            colVertices.Add(new Vector3(x, y, 0));
            colVertices.Add(new Vector3(x, y - 1, 0));

            ColliderTriangles();
            colCount++;
        }

        //right
        if (Block(x + 1, y) == 0)
        {
            colVertices.Add(new Vector3(x + 1, y, 1));
            colVertices.Add(new Vector3(x + 1, y - 1, 1));
            colVertices.Add(new Vector3(x + 1, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y, 0));

            ColliderTriangles();
            colCount++;
        }
    }

    // Creates indices for the last collider square
    void ColliderTriangles()
    {
        colTriangles.Add(colCount * 4);
        colTriangles.Add((colCount * 4) + 1);
        colTriangles.Add((colCount * 4) + 3);
        colTriangles.Add((colCount * 4) + 1);
        colTriangles.Add((colCount * 4) + 2);
        colTriangles.Add((colCount * 4) + 3);
    }

    // Checks the content of the indicated block. Stupid-ass name but wth. Tutorials yo
    byte Block(int x, int y)
    {
        // Out of bounds check. Just return air if we're outside the map
        if (x == -1 || x == blocks.GetLength(0) || y == -1 || y == blocks.GetLength(1))
        {
            return (byte)1;
        }

        return blocks[x, y];
    }

    // Custom perlin noise function. Apparently unity has its own but this is better? Somehow?
    int Noise(int x, int y, float scale, float mag, float exp)
    {
        return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * mag), (exp)));

    }

}
