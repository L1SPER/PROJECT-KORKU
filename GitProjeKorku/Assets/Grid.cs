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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        CreateFloor();
        FindStartNode();
    }

    private void FindStartNode()
    {
        gridFloor[x, y].floor.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {

    }
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
    void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
