using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
 using UnityEngine.UI;
public class InventorySystem : MonoBehaviour
{
 
    public GameObject ItemInfoUI;
    public static InventorySystem Instance { get; set; } 
    public GameObject inventoryScreenUI;
    public List<GameObject>slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;
    public bool isOpen;

    //PickUp Alert
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

    //public bool isFull;
 
 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
 
 
    void Start()
    {
        isOpen = false;

        PopulateSlotList();


        Cursor.visible = false;

    }


    private void PopulateSlotList()
    {
        foreach(Transform child in inventoryScreenUI.transform)
        {
            if(child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }


    void Update()
    {

     if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SaveLoadUI.Instance != null && SaveLoadUI.Instance.isOpen)
            {
                SaveLoadUI.Instance.CloseSaveLoad();
            }
            else if (isOpen)
            {
                CloseInventory();
            }
            else if (CraftingSystem.Instance.isOpen)
            {
                CraftingSystem.Instance.CloseCrafting();
            }
            else if (PauseMenu.Instance != null && PauseMenu.Instance.isPaused)
            {
                PauseMenu.Instance.ClosePauseMenu();
            }
            else if (PauseMenu.Instance != null)
            {
                PauseMenu.Instance.OpenPauseMenu();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
 
		    Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
 
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            isOpen = false;
        }
    }
    public void AddToInventory(string itemname)
{
            whatSlotToEquip = FindNextEmptySlot();

            if (whatSlotToEquip == null)
            {
                Debug.LogError("No empty slot found in inventory!");
                return;
            }

            itemToAdd = Instantiate(
                Resources.Load<GameObject>(itemname),
                whatSlotToEquip.transform.position,
                whatSlotToEquip.transform.rotation
            );

            itemToAdd.transform.SetParent(whatSlotToEquip.transform);
            itemList.Add(itemname);

            Debug.Log("Added to inventory: " + itemname);
            Debug.Log("Current itemList: " + string.Join(", ", itemList));

            TriggerPickupAlert(itemname, itemToAdd.GetComponent<Image>().sprite);
}

public void CloseInventory()
{
    inventoryScreenUI.SetActive(false);
    isOpen = false;

    //Ony hide cursor if NOTHING else is open
    if (!CraftingSystem.Instance.isOpen)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
    }
}

void TriggerPickupAlert(string itemName, Sprite itemSprite)
    {
        pickupName.text = itemName;

        pickupImage.sprite = itemSprite;

        pickupAlert.SetActive(true);
        
        StartCoroutine(HidePickupAlertAfterDelay(2f));
    }

    private IEnumerator HidePickupAlertAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        pickupAlert.SetActive(false);
    }
    private GameObject FindNextEmptySlot()
{
            foreach (GameObject slot in slotList)
            {
                if (slot.transform.childCount == 0)
                {
                    return slot;
                }
            }

            return null;
    }
            public bool CheckIfFull()
            {
                int counter = 0;
            
            foreach (GameObject slot in slotList)
                {
                    if (slot.transform.childCount >0)
                    {
                        counter += 1;
                    }
                }
                    if (counter == 21)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }      
    }
    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for (var i = slotList.Count - 1; i >= 0; i--)
        {
            if (counter == 0) break;
            
            if (slotList[i].transform.childCount > 0)
            {
                string childName = slotList[i].transform.GetChild(0).name;
                string cleanName = childName.Replace("(Clone)", "").Trim();
                
                if (cleanName == nameToRemove)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;
                }
            }
        }
        
        ReCalculateList();
    }
    public void ReCalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, ""); 
                itemList.Add(result);
            }
        }
    }
}