using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class Campfire : MonoBehaviour

{
public bool playerInRange;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && playerInRange)
        {
            // check if player is NOT holding axe
            bool holdingAxe = EquipSystem.Instance.selectedItem != null && 
                            EquipSystem.Instance.selectedItem.name.Contains("Axe");
                 Debug.Log("Left click detected! playerInRange: " + playerInRange);
                Debug.Log("Trying to cook!");
                Debug.Log("Has raw meat: " + InventorySystem.Instance.itemList.Contains("RawMeat"));

            if(!holdingAxe && InventorySystem.Instance.itemList.Contains("RawMeat"))
            {
                InventorySystem.Instance.RemoveItem("RawMeat", 1);
                InventorySystem.Instance.AddToInventory("CookedMeat");
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
