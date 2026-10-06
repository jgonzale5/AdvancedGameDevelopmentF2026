using UnityEngine;
using UnityEngine.Rendering;

//We require a mesh filter for what we're about to do
[RequireComponent(typeof(MeshFilter))]
public class TerrainGeneration : MonoBehaviour
{
    //The number of columns and rows of faces for this mesh
    [SerializeField]
    private int xFacecount = 5;
    [SerializeField]
    private int yFacecount = 5;

    //We can use this to dynamically determine the color of points on the mesh
    [SerializeField]
    private Gradient terrainColor;
    //The vertex colors of the mesh
    private Color[] vertexColors;

    //The highest y-value of the vertices
    [SerializeField]
    private float maxHeight = 2f;
    //A variable to scale the perlin noise up or down during generation
    [SerializeField]
    private float perlinMultiplier = 0.3f;
    //An offset on the perlin noise so it's not as obvious where the center of the terrain is
    [SerializeField]
    private Vector2 perlinOffset = Vector2.zero;

    //The filter that will be handling the mesh we generate
    private MeshFilter filter;
    //The mesh we're generating
    private Mesh mesh;

    //An array of vectors to keep track of the vertices of the mesh
    private Vector3[] vertices;
    //An array of integers to keep track of the triangles
    private int[] triangles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get a reference to the filter component in this game object
        filter = GetComponent<MeshFilter>();

        //Call the function to create a basic shape first
        //CreateBasicShape();
        CreateTerrain();
    }

    private void CreateBasicShape()
    {
        //For testing, we're gonna start by making a simple square. That's four vertices and two triangles
        vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1)
        };

        //We define two triangles. This array will be fed into the generation to form triangles with every three vertices.
        // So vertices[0], vertices[1], and vertices[2] form a triangle, so on
        triangles = new int[]
        {
            0, 1, 2,
            2, 1, 3
        };

        //Create a new mesh
        mesh = new Mesh();

        //Set the vertices on the mesh to the array of vector3, indicating the positions that those vertices will be at
        mesh.vertices = vertices;

        //Tell the mesh which of its vertices are connected through the triangles array
        mesh.triangles = triangles;

        //Calculate the normal vectors for the faces
        mesh.RecalculateNormals();

        //Set the mesh of the filter to the one we just made
        filter.mesh = mesh;
    }

    private void CreateTerrain()
    {
        //For every face we need two vertices,
        //since most faces share vertices we need one additional pair of vertices in each direction
        int vertexCount = (xFacecount + 1) * (yFacecount + 1);

        //An array to store the vertices
        vertices = new Vector3[vertexCount];

        //Create an array of the same size as the vertices
        vertexColors = new Color[vertexCount];

        //We use this to move through the array...
        int ind = 0;

        //We first create the vertices in a grid pattern
        for (int y = 0; y <= yFacecount; y++)
        {
            for (int x = 0; x <= xFacecount; x++)
            {
                //We set the height value based on the value of X and Y
                float height = Mathf.PerlinNoise( //Perlin Noise is a-
                    perlinOffset.x + x * perlinMultiplier, //We apply the offset, then scale the value
                    perlinOffset.y + y * perlinMultiplier); //We apply the offset, then scale the value

                //Get the color on the gradient corresponding to that value of the perling noise
                vertexColors[ind] = terrainColor.Evaluate(height);

                //Multiply the output by the height to get the height position of the vector
                height *= maxHeight;


                //We create a new vertex based on the value of the perlin noise and the current position
                vertices[ind] = new Vector3(x, height, y);

                //We increase the index to keep filling up the vertices array
                ind++;
            }
        }

        //Each triangle is 3 vertices
        //Each square is 2 triangles
        //There are xFacecount * yFacecount squares on the surface
        //So the number of triangles is V
        triangles = new int[xFacecount * yFacecount * 6];

        //We use these to keep track of triangles and vertices as we build the mesh
        int vert = 0;
        int tris = 0;

        for (int y = 0; y <  yFacecount; y++)
        {
            for (int x = 0; x < xFacecount ; x++)
            {
                //In this part we assign vertex indices to the triangles array
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xFacecount + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xFacecount + 1;
                triangles[tris + 5] = vert + xFacecount + 2;

                //Increase the number to move to the next set of vertices
                vert++;
                //Increase the number to move to the next two triangles
                tris += 6;
            }
            //Same as before
            vert++;
        }

        //This part is identical to the last function, it works the same way with different date
        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = vertexColors; //This is the one new line, where we set the color of the vertices
        mesh.SetColors(vertexColors);
        mesh.RecalculateNormals();
        filter.mesh = mesh;
    }

    //OnDrawGizmos is an editor callback function for debugging purposes
    //We're gonna have it draw the vertices so we can test this properly
    private void OnDrawGizmosSelected()
    {
        if (vertices == null) 
            return;

        foreach (Vector3 v in vertices)
        {
            Gizmos.DrawSphere(v, 0.1f);
        }
    }
}
