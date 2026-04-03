using UnityEngine;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine.Rendering;

public class LevelBuilder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    List<GameObject> generatedWalls = new List<GameObject>();
    public int xsquares = 7;
    public int zsquares = 7;
    public int startx = 3;
    public int starty = 3;
    float xmin, xmax, zmin, zmax;
    float blockwidth, blockheight;
    public void BuildMaze()
    {
        int[][] horizontalWalls = new int[zsquares][];
        for (int i = 0; i < zsquares; i++)
        {
            horizontalWalls[i] = new int[xsquares - 1];
            for (int j = 0; j < xsquares - 1; j++)
                horizontalWalls[i][j] = 3;
        }
        int[][] verticalWalls = new int[zsquares - 1][];
        for (int i = 0; i < zsquares - 1; i++)
        {
            verticalWalls[i] = new int[xsquares];
            for (int j = 0; j < xsquares; j++)
                verticalWalls[i][j] = 3;
        }
        List<Vector3Int> openList = new List<Vector3Int>();
        HashSet<Vector2Int> treeSquares = new HashSet<Vector2Int>();
        treeSquares.Add(new Vector2Int(startx, starty));
        foreach (Vector3Int v in AdjacentWalls(new Vector2Int(startx, starty)))
        {
            openList.Add(v);
            if (v.z == 0)
                horizontalWalls[v.x][v.y] = 2;
            else
                verticalWalls[v.x][v.y] = 2;
        }
        while (treeSquares.Count < xsquares * zsquares)
        {
            int index = Random.Range(0, openList.Count);
            Vector3Int wall = openList[index];
            openList.Remove(wall);
            Vector2Int squareOne = new Vector2Int(wall.x, wall.y);
            Vector2Int squareTwo = new Vector2Int(wall.x + wall.z, wall.y + (1 - wall.z));
            if (treeSquares.Contains(squareOne) && treeSquares.Contains(squareTwo))
            {
                if (wall.z == 0)
                    horizontalWalls[wall.x][wall.y] = 1;
                else
                    verticalWalls[wall.x][wall.y] = 1;
            }
            else if (!treeSquares.Contains(squareOne))
            {
                treeSquares.Add(squareOne);
                List<Vector3Int> neighbors = AdjacentWalls(squareOne);
                foreach (Vector3Int v in neighbors)
                {
                    if (v.z == 0 && horizontalWalls[v.x][v.y] == 3)
                    {
                        openList.Add(v);
                        horizontalWalls[v.x][v.y] = 2;
                    }
                    else if (v.z == 1 && verticalWalls[v.x][v.y] == 3)
                    {
                        openList.Add(v);
                        verticalWalls[v.x][v.y] = 2;
                    }
                }
                if (wall.z == 0)
                    horizontalWalls[wall.x][wall.y] = 0;
                else
                    verticalWalls[wall.x][wall.y] = 0;
            }
            else if (!treeSquares.Contains(squareTwo))
            {
                treeSquares.Add(squareTwo);
                List<Vector3Int> neighbors = AdjacentWalls(squareTwo);
                foreach (Vector3Int v in neighbors)
                {
                    if (v.z == 0 && horizontalWalls[v.x][v.y] == 3)
                    {
                        openList.Add(v);
                        horizontalWalls[v.x][v.y] = 2;
                    }
                    else if (v.z == 1 && verticalWalls[v.x][v.y] == 3)
                    {
                        openList.Add(v);
                        verticalWalls[v.x][v.y] = 2;
                    }
                }
                if (wall.z == 0)
                    horizontalWalls[wall.x][wall.y] = 0;
                else
                    verticalWalls[wall.x][wall.y] = 0;
            }
        }
        GenerateMaze(horizontalWalls, verticalWalls);
    }

    public List<Vector3Int> AdjacentWalls(Vector2Int square)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        if (square.x > 0) neighbors.Add(new Vector3Int(square.x - 1, square.y, 1));
        if (square.y > 0) neighbors.Add(new Vector3Int(square.x, square.y - 1, 0));
        if (square.x < xsquares - 1) neighbors.Add(new Vector3Int(square.x, square.y, 1));
        if (square.y < zsquares - 1) neighbors.Add(new Vector3Int(square.x, square.y, 0));
        return neighbors;
    }
    public void ClearMaze()
    {
        foreach (GameObject x in generatedWalls)
            Destroy(x);
        generatedWalls.Clear();
    }
    public void RebuildMaze()
    {
        ClearMaze();
        Bounds mazeBounds = GetBounds();
        xmin = mazeBounds.min.x + 0.25f;
        xmax = mazeBounds.max.x - 0.25f;
        zmin = mazeBounds.min.z + 0.25f;
        zmax = mazeBounds.max.z - 0.25f;
        blockwidth = (xmax - xmin) / xsquares;
        blockheight = (zmax - zmin) / zsquares;
        BuildMaze();
    }

    //Precondition: horizontalWalls and verticalWalls contain the layout of the maze in the format:
    //              horizontalWalls[i][j] is 0 if the wall between square (i,j) and (i,j+1) is gone, nonzero otherwise
    //              verticalWalls[i][j] is 0 if the wall between square (i,j) and (i+1,j) is gone, nonzero otherwise
    //Postcondition: All walls have been generated, and added to the generatedWalls list.
    //Note:         To create a cube without a prefab, you can use GameObject.CreatePrimitive(PrimitiveType.Cube);

    public void GenerateMaze(int[][] horizontalWalls, int[][] verticalWalls)
    {
        print("genWalls: " + generatedWalls.Count);

        //if (xsquares == zsquares)
        //{

        //    for (int i = 0; i < horizontalWalls.Length - 1; i++)
        //    {

        //        //print("this is i: " + i);
        //        for (int k = 0; k < horizontalWalls.Length - 1; k++)
        //        {
        //            //print("this is k: " + k);
        //            if (horizontalWalls[i][k] != 0)
        //            {
        //                int length = CheckNextHorizontalWall(i,k, horizontalWalls);
        //                GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //                wall.name = "horizontal wall";
        //                wall.transform.localScale = new Vector3(blockwidth + 0.25f, 5, 0.25f);
        //                wall.transform.position = new Vector3((i + 0.5f) * blockwidth + xmin, 2, (k + 1f) * blockheight + zmin);
        //                generatedWalls.Add(wall);
        //            }
        //            if (verticalWalls[i][k] != 0)
        //            {
        //                GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //                wall.name = "vertical wall";
        //                wall.transform.localScale = new Vector3(0.25f, 5, blockheight + 0.25f);
        //                wall.transform.position = new Vector3((i + 1f) * blockwidth + xmin, 2, (k + 0.5f) * blockheight + zmin);
        //                generatedWalls.Add(wall);
        //            }
        //        }
        //    }


        //}
        //else
        //{

        for (int i = 0; i < horizontalWalls.Length - 1; i++)
        {
            for (int k = 0; k < horizontalWalls.Length - 1; k++)
            {
                if (horizontalWalls[k][i] != 0)
                {
                    //print("Checking wall (" + k + ", " + i + ")");
                    //int length = CheckNextHorizontalWall(k, i, horizontalWalls);
                    //print("length " + length);
                    //k += length;
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.name = "horizontal wall(" + k + ", " + i + ")";
                    //wall.transform.localScale = length != 0 ?  new Vector3(blockwidth * length + 0.25f, 5, 0.25f) : new Vector3(blockwidth + 0.25f, 5, 0.25f);
                    wall.transform.localScale = new Vector3(blockwidth + 0.25f, 5, 0.25f);
                    wall.transform.position = new Vector3((k + 0.5f) * blockwidth + xmin, 2, (i + 1f) * blockheight + zmin);
                    wall.transform.SetParent(GameObject.Find("Inside Walls").transform, false);
                    generatedWalls.Add(wall);
                }
            }
        }

        for (int i = 0; i < verticalWalls.Length - 1; i++)
            {
                for (int k = 0; k <= verticalWalls.Length - 1; k++)
                {
                    if (verticalWalls[i][k] != 0)
                    {
                        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        wall.name = "vertical wall";
                        wall.transform.localScale = new Vector3(0.25f, 5, blockheight + 0.25f);
                        wall.transform.position = new Vector3((i + 1f) * blockwidth + xmin, 2, (k + 0.5f) * blockheight + zmin);
                        wall.transform.SetParent(GameObject.Find("Inside Walls").transform, false);
                        generatedWalls.Add(wall);
                    }
                }
            }
        //}
    }

    private int CheckNextHorizontalWall(int k, int i, int[][] horizontalWalls)
    {
        int numWalls = 0;
        if (k + 1 >= horizontalWalls.GetLength(0) || horizontalWalls[k + 1][i] != 0)
        {
            return numWalls;
        }
        else
        {
            numWalls += 1 + CheckNextHorizontalWall(k + 1, i, horizontalWalls);
            return numWalls;
        }
       
    }
    void Start()
    {
        Bounds mazeBounds = GetBounds();
        xmin = mazeBounds.min.x + 0.25f;
        xmax = mazeBounds.max.x - 0.25f;
        zmin = mazeBounds.min.z + 0.25f;
        zmax = mazeBounds.max.z - 0.25f;
        blockwidth = (xmax - xmin) / xsquares;
        blockheight = (zmax - zmin) / zsquares;

        BuildMaze();
    }

    public Bounds GetBounds()
    {
        Bounds bounds = new Bounds();
        foreach (MeshRenderer childmesh in GetComponentsInChildren<MeshRenderer>())
        {
            bounds.Encapsulate(childmesh.bounds);
        }
        return bounds;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
