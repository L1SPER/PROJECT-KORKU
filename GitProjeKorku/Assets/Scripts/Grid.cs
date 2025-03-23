using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] Transform parentFloorTransform;
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
    }

    void Update()
    {
        UpdateFloor();
    }

    /// <summary>
    /// Grid oluşturulur.
    /// </summary>
    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                grid[x, y] = new Node(new Floor(null, FloorType.White), worldPoint, x, y);
            }
        }
    }
    /// <summary>
    /// Zemin oluşturulur.
    /// </summary>
    private void CreateFloor()
    {
        gridFloor = new Node[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                GameObject _floor = Instantiate(whiteFloor, worldPoint, Quaternion.identity);
                gridFloor[x, y] = new Node(worldPoint, x, y, _floor, FloorType.White);
                _floor.transform.SetParent(parentFloorTransform);
            }
        }
    }
    /// <summary>
    /// Zemin noktası güncellenir.
    /// </summary>
    private void UpdateFloor()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                gridFloor[x, y] = new Node(grid[x, y].worldPosition, x, y, gridFloor[x, y].floor.floorGameObject, grid[x, y].floor.floorType);
                if (gridFloor[x, y].floor.floorType == FloorType.Red)
                {
                    gridFloor[x, y].floor.floorGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else if (gridFloor[x, y].floor.floorType == FloorType.White)
                {
                    gridFloor[x, y].floor.floorGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
                }
                else if (gridFloor[x, y].floor.floorType == FloorType.Yellow)
                {
                    gridFloor[x, y].floor.floorGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.yellow;
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
        int x = (int)(worldPosition.x - bottomLeft.x) / (int)nodeDiameter;
        int y = (int)(worldPosition.z - bottomLeft.z) / (int)nodeDiameter;
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
        int xLocal = (int)(room.transform.GetChild(0).GetComponent<Room>().size.x / nodeDiameter);
        int zLocal = (int)(room.transform.GetChild(0).GetComponent<Room>().size.z / nodeDiameter);

        for (int i = 0; i < xLocal; i++)
        {
            for (int j = 0; j < zLocal; j++)
            {
                if (grid[xRandom + i, zRandom + j].floor.floorType == FloorType.Red)
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
        int xLocal = (int)(room.transform.GetChild(0).GetComponent<Room>().size.x / nodeDiameter);
        int zLocal = (int)(room.transform.GetChild(0).GetComponent<Room>().size.z / nodeDiameter);

        for (int i = 0; i < xLocal; i++)
        {
            for (int j = 0; j < zLocal; j++)
            {
                grid[xRandom + i, zRandom + j].floor.floorType = FloorType.Red;
                gridFloor[xRandom + i, zRandom + j].floor.floorType = FloorType.Red;
                FloorManager.AddRedFloor(grid[xRandom + i, zRandom + j]);
            }
        }
    }
    /// <summary>
    /// Komşular bulunur.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
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
    public Node GetNeighbor(Node node, Vector2 pos)
    {
        Vector2 currentPos = new Vector2(node.gridX, node.gridY) + pos;
        return grid[(int)currentPos.x, (int)currentPos.y];
    }
    public FloorType [] GetNeighborColors(Node node)
    {
        FloorType [] floorTypes= new FloorType[4];
        floorTypes[0] = GetNeighbor(node, Vector2.up)!= null ? GetNeighbor(node, Vector2.up).floor.floorType:FloorType.Red;
        floorTypes[1] = GetNeighbor(node, Vector2.right)!= null ? GetNeighbor(node, Vector2.right).floor.floorType:FloorType.Red;
        floorTypes[2] = GetNeighbor(node, Vector2.down)!= null ? GetNeighbor(node, Vector2.down).floor.floorType:FloorType.Red;
        floorTypes[3] = GetNeighbor(node, Vector2.left)!= null ? GetNeighbor(node, Vector2.left).floor.floorType:FloorType.Red;
        return floorTypes;
    }
}
