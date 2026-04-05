using UnityEngine;
using System.Collections;

public class Campfire : MonoBehaviour
{
    public static Campfire Instance;
    public bool playerInRange;
    private bool isCooking = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && playerInRange && !isCooking)
        {
            {
                Debug.Log("Click! isCooking: " + isCooking);
                Debug.Log("itemList contains RawMeat: " + InventorySystem.Instance.itemList.Contains("RawMeat"));
                Debug.Log("Full list: " + string.Join(", ", InventorySystem.Instance.itemList));
            }
            bool holdingAxe = EquipSystem.Instance.selectedItem != null && 
                            EquipSystem.Instance.selectedItem.name.Contains("Axe");

            if(!holdingAxe && InventorySystem.Instance.itemList.Contains("RawMeat"))
            {
                StartCoroutine(CookMeat());
            }
        }
    }

    IEnumerator CookMeat()
    {
        isCooking = true;
        
        if (!InventorySystem.Instance.itemList.Contains("RawMeat"))
        {
            isCooking = false;
            yield break;
        }
        
        InventorySystem.Instance.RemoveItem("RawMeat", 1);
        yield return new WaitForSeconds(1f);
        InventorySystem.Instance.AddToInventory("CookedMeat");
        isCooking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}