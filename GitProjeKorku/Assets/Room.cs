using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject doorTriggerPrefab;
    [SerializeField] Transform doorTriggerPrefabParentTransform;

    [SerializeField] GameObject doorPrefab;
    [SerializeField] Door[] doors;
    [SerializeField] Transform parentTransform;
    [SerializeField] Vector3[] doorPositions;
    //[SerializeField] Vector3[] quaternion;

    void Awake()
    {
        InstantiateDoors();
    }
    private void InstantiateDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            /* GameObject door= Instantiate(doorPrefab,parentTransform) as GameObject;
            door.transform.position = this.transform.position + doorPositions[i];
            door.transform.rotation = Quaternion.Euler(quaternion[i]);
            doors[i].doorMesh= door; 
            doors[i].isOpen = false;
            doors[i].isIntersect = false; */
            GameObject door= Instantiate(doorPrefab) as GameObject;
            door.transform.SetParent(parentTransform);
            door.transform.position = this.transform.position + doorPositions[i];

            GameObject triggerPrefab= Instantiate(doorTriggerPrefab) as GameObject;
            triggerPrefab.transform.SetParent(doorTriggerPrefabParentTransform);
            triggerPrefab.transform.position =door.transform.position ;

            //door.transform.rotation = Quaternion.Euler(quaternion[i]);
            //doors[i].doorMesh= door; 
            doors[i].isOpen = false;
            doors[i].isIntersect = false;
        }
         
    }
  
}
