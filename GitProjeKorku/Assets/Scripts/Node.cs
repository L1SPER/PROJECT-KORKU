using UnityEngine;

public class Node 
{
    public GameObject floor;
    public bool walkable;
    public Vector3 worldPosition;

    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;
    public FloorType floorType;
    public Node(bool _walkable, Vector3 _worldPost, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPost;
        gridX = _gridX;
        gridY = _gridY;
    }
    public Node(bool _walkable, Vector3 _worldPost, int _gridX, int _gridY,FloorType _floorType)
    {
        walkable = _walkable;
        floorType = _floorType;
        worldPosition = _worldPost;
        gridX = _gridX;
        gridY = _gridY;
    }
    public Node(GameObject _floor, bool _walkable, Vector3 _worldPost, int _gridX, int _gridY)
    {
        floor = _floor;
        walkable = _walkable;
        worldPosition = _worldPost;
        gridX = _gridX;
        gridY = _gridY;
    }
    public Node(GameObject _floor, bool _walkable, Vector3 _worldPost, int _gridX, int _gridY,FloorType _floorType)
    {
        floor = _floor;
        floorType = _floorType;
        walkable = _walkable;
        worldPosition = _worldPost;
        gridX = _gridX;
        gridY = _gridY;
    }
    public int fCost
    {
        get { return gCost + hCost; }
    }
}
public enum FloorType
{
    White,// Walkable
    Red,// Unwalkable
    Yellow,//Path
}
