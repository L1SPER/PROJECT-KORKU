using System;
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
    Vector3 bottomLeft;

    [SerializeField] int x, y;
    [SerializeField] private float offset;
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        CreateFloor();
        FindStartNode();
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
    }
    /// <summary>
    /// Grid oluşturulur.
    /// </summary>
    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        Debug.Log("En sol alt kose: " + bottomLeft);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }
    /// <summary>
    /// Zemin oluşturulur.
    /// </summary>
    private void CreateFloor()
    {
        gridFloor = new Node[gridSizeX, gridSizeY];
        bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
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
                    gridFloor[x, y] = new Node(floor, walkable, worldPoint, x, y);
                    floor.transform.SetParent(parentFloorTransform);
                }
                else
                {
                    GameObject floor = Instantiate(redFloor, worldPoint, Quaternion.identity);
                    gridFloor[x, y] = new Node(floor, walkable, worldPoint, x, y);
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
        bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        Debug.Log("En sol alt kose: " + bottomLeft);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius-offset, unwalkableMask);
                if (walkable)
                {
                    gridFloor[x, y].floor.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
                }
                else
                {
                    gridFloor[x, y].floor.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
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
    /// <summary>
    /// Oda uygun mu kontrol edilir.
    /// </summary>
    /// <param name="xRandom"></param>
    /// <param name="zRandom"></param>
    /// <param name="room"></param>
    /// <returns></returns>
    public bool CheckIfRoomFits(int xRandom, int zRandom, GameObject room)
    {
        int xLocal = (int)(room.transform.GetChild(0).transform.localScale.x/nodeDiameter);
        int zLocal = (int)(room.transform.GetChild(0).transform.localScale.z/nodeDiameter);

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
        int xLocal = (int)(room.transform.GetChild(0).transform.localScale.x/nodeDiameter);
        int zLocal = (int)(room.transform.GetChild(0).transform.localScale.z/nodeDiameter);

        for (int i = 0; i <= xLocal+1; i++)
        {
            for (int j = 0; j <= zLocal+1; j++)
            {
                grid[xRandom + i-1, zRandom + j-1].walkable = false;
                gridFloor[xRandom + i-1, zRandom + j-1].walkable=false;
            }
        }
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
