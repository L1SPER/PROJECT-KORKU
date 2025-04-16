using System.Collections.Generic;
using UnityEngine;

public class FloorManager: MonoBehaviour
{
    public List<Node> yellowFloors = new List<Node>();
    public List<Node> redFloors = new List<Node>();

    public void AddYellowFloor(Node yellowFloor)
    {
        if (yellowFloor.floor.inTheList)
            return;
        yellowFloors.Add(yellowFloor);
        yellowFloor.floor.inTheList = true;
    }
    public void AddRedFloor(Node redFloor)
    {
        if (redFloor.floor.inTheList)
            return;
        redFloors.Add(redFloor);
        redFloor.floor.inTheList = true;
    }
    public void RemoveYellowFloor(Node yellowFloor)
    {
        yellowFloors.Remove(yellowFloor);
        yellowFloor.floor.inTheList = false;
    }
    public void RemoveRedFloor(Node redFloor)
    {
        redFloors.Remove(redFloor);
        redFloor.floor.inTheList = false;
    }
    public List<Node> GetYellowFloors()
    {
        return yellowFloors;
    }
     public List<Node> GetRedFloors()
    {
        return redFloors;
    }
}
