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
        Debug.Log("Items before remove: " + string.Join(", ", InventorySystem.Instance.itemList));
        InventorySystem.Instance.RemoveItem("RawMeat", 1);
        Debug.Log("Items after remove: " + string.Join(", ", InventorySystem.Instance.itemList));
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