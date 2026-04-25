using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

public bool playerInRange;


 public string ItemName;

    public string GetItemName()
    {
        return ItemName;
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.E))
    {
        Debug.Log($"E pressed!");
        Debug.Log($"playerInRange: {playerInRange}");
        Debug.Log($"onTarget: {SelectionManager.Instance.onTarget}");
        Debug.Log($"selectedObject == this: {SelectionManager.Instance.selectedObject == gameObject}");
        Debug.Log($"Inventory full: {InventorySystem.Instance.CheckIfFull()}");
    }

    if (Input.GetKeyDown(KeyCode.E) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
    {
        if(!InventorySystem.Instance.CheckIfFull())
        {
            InventorySystem.Instance.AddToInventory(ItemName);
            Destroy(gameObject);
        }
        else
        { 
            Debug.Log("Inventory is full");
        }
    }
}



    private void OnTriggerEnter(Collider other)
    {
        
      if (other.CompareTag("Player"))
        {
             Debug.Log("Trigger entered by: " + other.gameObject.name);
            playerInRange = true;

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            playerInRange = false;
        }
    }
}