using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] Transform parentFloorTransform;
    [SerializeField] GameObject redFloor;
    [SerializeField] GameObject whiteFloor;

    [SerializeField] private LayerMask unwalkableMask;
    [SerializeField] public Vector2 gridWorldSize;
    [SerializeField] public float nodeRadius;
    public Node[,] grid;
    public Node[,] gridFloor;

    public float nodeDiameter;
    public int gridSizeX, gridSizeY;
    [SerializeField] Vector3 bottomLeft;

    [SerializeField] int x, y;
    [SerializeField] private float offset;
    int counter = 0;
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        CreateFloor();
        CheckGrid();
        //FindStartNode();
    }

    /// <summary>
    /// Başlangıç noktası bulunur.
    /// </summary>
    private void FindStartNode()
    {
        gridFloor[x, y].floor.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void Update()
    {
        UpdateFloor();
        //CheckGrid();
    }
    private void CheckGrid()
    {
        for(int i=0; i<gridSizeX; i++)
        {
            for(int j=0; j<gridSizeY; j++)
            {
                if(grid[i,j]==null)
                {
                    Debug.Log("counter"+counter);
                    counter++;
                }
            }
        }
    }

    /// <summary>
    /// Grid oluşturulur.
    /// </summary>
    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        //bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        Debug.Log("En sol alt kose: " + bottomLeft);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                //grid[x, y] = new Node(walkable, worldPoint, x, y, FloorType.White);
                if(walkable)
                {
                    grid[x, y] = new Node(walkable, worldPoint, x, y, FloorType.White);
                }
                else
                {
                    grid[x, y] = new Node(walkable, worldPoint, x, y, FloorType.Red);
                }
                //grid[x, y] = new Node(walkable, worldPoint, x, y, FloorType.White);
            }
        }
    }
    /// <summary>
    /// Zemin oluşturulur.
    /// </summary>
    private void CreateFloor()
    {
        gridFloor = new Node[gridSizeX, gridSizeY];
        //bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        Debug.Log("En sol alt kose: " + bottomLeft);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                if (walkable)
                {
                    GameObject floor = Instantiate(whiteFloor, worldPoint, Quaternion.identity);
                    gridFloor[x, y] = new Node(floor, walkable, worldPoint, x, y, FloorType.White);
                    floor.transform.SetParent(parentFloorTransform);
                }
                else
                {
                    GameObject floor = Instantiate(redFloor, worldPoint, Quaternion.identity);
                    gridFloor[x, y] = new Node(floor, walkable, worldPoint, x, y, FloorType.Red);
                    floor.transform.SetParent(parentFloorTransform);
                }
            }
        }
    }
    /// <summary>
    /// Zemin noktası güncellenir.
    /// </summary>
    private void UpdateFloor()
    {
        //bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        Debug.Log("En sol alt kose: " + bottomLeft);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                /* Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask); */
                gridFloor[x, y]=new Node(gridFloor[x, y].floor, grid[x, y].walkable, grid[x, y].worldPosition, x, y, grid[x, y].floorType);
                if(gridFloor[x, y].floorType==FloorType.Red)
                {
                    gridFloor[x, y].floor.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else if(gridFloor[x, y].floorType==FloorType.White)
                {
                    gridFloor[x, y].floor.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
                }
                else if(gridFloor[x, y].floorType==FloorType.Yellow)
                {
                    gridFloor[x, y].floor.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
            }
        }
    }
    /// <summary>
    /// Grid dünyasındaki noktanın dünya noktası hesaplanır.
    /// </summary>
    /// <param name="xRandom"></param>
    /// <param name="zRandom"></param>
    /// <returns></returns>
    public Vector3 CalculateWorldPoint(int xRandom, int zRandom)
    {
        return bottomLeft + Vector3.right * (xRandom * nodeDiameter + nodeRadius) + Vector3.forward * (zRandom * nodeDiameter + nodeRadius);
    }
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        int x= (int) (worldPosition.x - bottomLeft.x) / (int) nodeDiameter;
        int y= (int) (worldPosition.z - bottomLeft.z) / (int) nodeDiameter;
        return grid[x, y]; 
    }
    /// <summary>
    /// Oda uygun mu kontrol edilir.
    /// </summary>
    /// <param name="xRandom"></param>
    /// <param name="zRandom"></param>
    /// <param name="room"></param>
    /// <returns></returns>
    public bool CheckIfRoomFits(int xRandom, int zRandom, GameObject room)
    {
        int xLocal = (int) (room.transform.GetChild(0).GetComponent<Room>().size.x /nodeDiameter);
        int zLocal = (int) (room.transform.GetChild(0).GetComponent<Room>().size.z /nodeDiameter);

        for (int i = 0; i < xLocal; i++)
        {
            for (int j = 0; j < zLocal; j++)
            {
                if (grid[xRandom + i, zRandom + j].walkable == false)
                {
                    return false;
                }
            }
        }
        return true;
    }
    /// <summary>
    /// Oda çizilir.
    /// </summary>
    /// <param name="xRandom"></param>
    /// <param name="zRandom"></param>
    /// <param name="room"></param>
    public void DrawRoom(int xRandom, int zRandom, GameObject room)
    {
        int xLocal = (int)( room.transform.GetChild(0).GetComponent<Room>().size.x/nodeDiameter);
        int zLocal = (int)( room.transform.GetChild(0).GetComponent<Room>().size.z/nodeDiameter);

        for (int i = 0; i < xLocal; i++)
        {
            for (int j = 0; j < zLocal; j++)
            {
                grid[xRandom + i, zRandom + j].walkable = false;
                gridFloor[xRandom + i, zRandom + j].walkable=false;
                grid[xRandom + i, zRandom + j].floorType=FloorType.Red;
                gridFloor[xRandom + i, zRandom + j].floorType=FloorType.Red;
            }
        }
    }
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        int[,] directions = new int[,]
        {
            { 0, 1 },  // Yukarı
            { 0, -1 }, // Aşağı
            { -1, 0 }, // Sol
            { 1, 0 }   // Sağ
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int checkX = node.gridX + directions[i, 0];
            int checkY = node.gridY + directions[i, 1];

            if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
            {
                neighbors.Add(grid[checkX, checkY]);
            }
        }

        return neighbors;
    }

    /* void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    } */
}
