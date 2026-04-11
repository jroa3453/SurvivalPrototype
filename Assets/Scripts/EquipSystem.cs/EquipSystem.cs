using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    [Header("UI")]
    public GameObject quickSlotsPanel;
    public GameObject numbersHolder;

    [Header("Tool")]
    public GameObject toolHolder;

    [Header("Equipped Model Offset")]
    public Vector3 modelPosition = new Vector3(0.3f, -0.3f, 0.6f);
    public Vector3 modelRotation = new Vector3(0f, 0f, 0f);
    public Vector3 modelScale    = new Vector3(1f, 1f, 1f);
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip equipSound;
    public List<GameObject> quickSlotsList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    public int selectedNumber = -1;
    public GameObject selectedItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        PopulateSlotList();
    }

    void Update()
    {
        if      (Input.GetKeyDown(KeyCode.Alpha1)) SelectQuickSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SelectQuickSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SelectQuickSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SelectQuickSlot(4);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) SelectQuickSlot(5);
        else if (Input.GetKeyDown(KeyCode.Alpha6)) SelectQuickSlot(6);
        else if (Input.GetKeyDown(KeyCode.Alpha7)) SelectQuickSlot(7);

        // Live update equipped model transform while in Play mode
        if (toolHolder != null && toolHolder.transform.childCount > 0)
        {
            Transform equippedModel = toolHolder.transform.GetChild(0);
            equippedModel.localPosition = modelPosition;
            equippedModel.localRotation = Quaternion.Euler(modelRotation);
            equippedModel.localScale    = modelScale;
        }
    }

    // ✅ Fixed: was item.Damage (capital D), now matches the field name in EquippableItem
    public int GetWeaponDamage()
    {
        if (selectedItem != null)
        {
            EquippableItem item = selectedItem.GetComponent<EquippableItem>();
            if (item != null) return item.damage;
        }
        return 0;
    }

    public bool IsHoldingWeapon()
    {
        return selectedItem != null && selectedItem.GetComponent<EquippableItem>() != null;
    }

    void SelectQuickSlot(int number)
    {
        if (!CheckIfSlotIsFull(number)) return;

        // Press same number again = deselect
        if (selectedNumber == number)
        {
            DeselectCurrentItem();
            return;
        }

        // Deselect previous
        if (selectedItem != null)
        {
            InventoryItem oldItem = selectedItem.GetComponent<InventoryItem>();
            if (oldItem != null) oldItem.isSelected = false;
        }

        // Select new
        selectedNumber = number;
        selectedItem   = GetSelectedItem(number);

        InventoryItem newItem = selectedItem.GetComponent<InventoryItem>();
        if (newItem != null) newItem.isSelected = true;

        SetEquippedModel(selectedItem);
        UpdateNumberHighlight(number);
    }

    void DeselectCurrentItem()
    {
        selectedNumber = -1;

        if (selectedItem != null)
        {
            InventoryItem item = selectedItem.GetComponent<InventoryItem>();
            if (item != null) item.isSelected = false;
            selectedItem = null;
        }

        ClearEquippedModel();
        ResetAllNumberHighlights();
    }

    void SetEquippedModel(GameObject item)
    {
        if (item == null)       { Debug.LogError("SetEquippedModel: item is null");       return; }
        if (toolHolder == null) { Debug.LogError("SetEquippedModel: toolHolder is null"); return; }

        ClearEquippedModel();

        string itemName = item.name.Replace("(Clone)", "").Trim();
        GameObject loadedModel = Resources.Load<GameObject>(itemName + "_Model");

        if (loadedModel == null)
        {
            Debug.LogError("Could not find model in Resources: " + itemName + "_Model");
            return;
        }

        GameObject itemModel = Instantiate(loadedModel, toolHolder.transform);
        itemModel.transform.localPosition = modelPosition;
        itemModel.transform.localRotation = Quaternion.Euler(modelRotation);
        itemModel.transform.localScale    = modelScale;

       audioSource.PlayOneShot(equipSound);
    }

    void ClearEquippedModel()
    {
        foreach (Transform child in toolHolder.transform)
            Destroy(child.gameObject);
    }

    void UpdateNumberHighlight(int selectedNum)
    {
        ResetAllNumberHighlights();

        Transform numberTransform = numbersHolder.transform.Find("number" + selectedNum);
        if (numberTransform == null) { Debug.LogError("Could not find: number" + selectedNum); return; }

        Transform textTransform = numberTransform.Find("Text");
        if (textTransform == null) { Debug.LogError("Could not find Text under number" + selectedNum); return; }

        Text text = textTransform.GetComponent<Text>();
        if (text != null) text.color = Color.white;
    }

    void ResetAllNumberHighlights()
    {
        foreach (Transform child in numbersHolder.transform)
        {
            Transform textTransform = child.Find("Text");
            if (textTransform != null)
            {
                Text text = textTransform.GetComponent<Text>();
                if (text != null) text.color = Color.gray;
            }
        }
    }

    GameObject GetSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject;
    }

    bool CheckIfSlotIsFull(int slotNumber)
    {
        return quickSlotsList[slotNumber - 1].transform.childCount > 0;
    }

    void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
                quickSlotsList.Add(child.gameObject);
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        GameObject availableSlot = FindNextEmptySlot();
        if (availableSlot == null) { Debug.Log("No empty quick slots!"); return; }

        itemToEquip.transform.SetParent(availableSlot.transform, false);
        itemList.Add(itemToEquip.name.Replace("(Clone)", "").Trim());
        InventorySystem.Instance.ReCalculateList();
    }

    GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
                return slot;
        }
        return null;
    }

    public bool CheckIfFull()
    {
        int count = 0;
        foreach (GameObject slot in quickSlotsList)
            if (slot.transform.childCount > 0) count++;
        return count == 7;
    }

    public void RemoveFromQuickSlots(string itemName)
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0) continue;

            GameObject item = slot.transform.GetChild(0).gameObject;
            string cleanName = item.name.Replace("(Clone)", "").Trim();

            if (cleanName == itemName)
            {
                Destroy(item);
                itemList.Remove(itemName);
                InventorySystem.Instance.ReCalculateList();
                ClearEquippedModel();
                return;
            }
        }
    }
}