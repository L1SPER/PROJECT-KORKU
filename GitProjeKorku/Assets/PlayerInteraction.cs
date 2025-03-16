using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private Transform playerCamera;
    private IInteractable lastInteractableObj;
    private bool canHit;

    private void Start()
    {
        lastInteractableObj = null;
        canHit = true;
    }
    private void Update()
    {
        Shoot();
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward*10, Color.green);     
    }
  
    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            IInteractable InteractableObj = hit.transform.GetComponent<IInteractable>();
            if (InteractableObj != null && canHit)
            {
                if (InteractableObj != lastInteractableObj)
                {
                    //Hitting to a new object
                    if (lastInteractableObj != null)
                    {
                        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward*10, Color.red);     
                        lastInteractableObj.InteractWithoutPressingButton();
                    }
                }
                lastInteractableObj = InteractableObj;
                InteractableObj.InteractWithoutPressingButton(true);
                if (Input.GetButtonDown("Fire1"))
                {
                    InteractableObj.InteractWithPressingButton();
                    lastInteractableObj = null;
                }
            }
        }
        else
        {
            if (lastInteractableObj != null)
            {
                lastInteractableObj.InteractWithoutPressingButton();
                lastInteractableObj = null;
            }
        }
    }
}