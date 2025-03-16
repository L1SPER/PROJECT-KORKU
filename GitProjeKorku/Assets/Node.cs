using UnityEngine;

public class Node : MonoBehaviour
{
    public GameObject floor;
    public bool walkable;
    public Vector3 worldPosition;

    public int gridX;
    public int gridY;

    public Node(bool _walkable, Vector3 _worldPost, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPost;
        gridX = _gridX;
        gridY = _gridY;
    }
     public Node(GameObject _floor,bool _walkable, Vector3 _worldPost, int _gridX, int _gridY)
    {
        floor= _floor;
        walkable = _walkable;
        worldPosition = _worldPost;
        gridX = _gridX;
        gridY = _gridY;
    }
}
