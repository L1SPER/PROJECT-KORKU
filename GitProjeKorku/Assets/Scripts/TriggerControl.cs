using System;
using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    Collider[] colliders;
    void Start()
    {
        Control();
        DisableColliders();
    }

    private void DisableColliders()
    {
        if (colliders.Length > 1) // 1 ise sadece kendisini algılamıştır
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].gameObject.GetComponent<Door>().isIntersect) 
                {
                    colliders[i].enabled = false;
                }
            }
            
        }
    }

    private void Control()
    {
        colliders = Physics.OverlapBox(transform.position, new Vector3(1.1f,1.1f, 1.1f), Quaternion.identity, LayerMask.GetMask("Door"));
        if (colliders.Length > 1) // 1 ise sadece kendisini algılamıştır
        {
            if(isIntersectDoors(colliders))
            {
                SetDoorsIsOpenTrue(colliders);
            }
        }
    }
    private bool isIntersectDoors(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.GetComponent<Door>().isOpen)
            {
                return true;
            }
        }
        return false;
    }

    private void SetDoorsIsOpenTrue(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].gameObject.GetComponent<Door>().isIntersect = true;
            colliders[i].enabled = false;
            colliders[i].gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }
}
