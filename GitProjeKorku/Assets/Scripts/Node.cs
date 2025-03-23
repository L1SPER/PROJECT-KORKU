using UnityEngine;

public class Node 
{
    public Vector3 worldPosition;

    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;
    public Floor floor;
    public Node( Vector3 _worldPost, int _gridX, int _gridY)
    {
        worldPosition = _worldPost;
        gridX = _gridX;
        gridY = _gridY;
    }
    public Node(Vector3 _worldPost, int _gridX, int _gridY, GameObject _floor, FloorType _floorType)
    {
        worldPosition = _worldPost;
        gridX = _gridX;
        gridY = _gridY;
        floor = new Floor(_floor, _floorType);
    }
    public Node(Floor _floor,Vector3 _worldPost, int _gridX, int _gridY)
    {
        worldPosition = _worldPost;
        gridX = _gridX;
        gridY = _gridY;
        floor = _floor;
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
    None,
}
