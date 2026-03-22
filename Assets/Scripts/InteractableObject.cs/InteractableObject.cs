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
        
       if (Input.GetKeyDown(KeyCode.E) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
       {
            //if the inventory is not full then add the item to the inventory and destroy the item in the world
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