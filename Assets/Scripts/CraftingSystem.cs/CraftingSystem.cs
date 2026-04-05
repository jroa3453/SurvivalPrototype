using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; set; }

    [Header("Main UI Screens")]
    [SerializeField] private GameObject craftingScreenUI;
    [SerializeField] private GameObject toolsScreenUI;
    [SerializeField] private GameObject survivalScreenUI;
    [SerializeField] private GameObject refineScreenUI;

    [Header("Category Screens")]
    [SerializeField] private GameObject ToolsCategoryScreen;
    [SerializeField] private GameObject SurvivalCategoryScreen;
    [SerializeField] private GameObject RefineCategoryScreen;

    [Header("Category Buttons")]
    [SerializeField] private Button toolsBTN;
    [SerializeField] private Button survivalBTN;
    [SerializeField] private Button refineBTN;

    [Header("Craft Buttons")]
    [SerializeField] private Button craftAxeBTN;
    [SerializeField] private Button craftPlankBTN;
    [SerializeField] private Button craftBlueprintBTN;

    [Header("Requirement Text")]
    [SerializeField] private Text AxeReq1;
    [SerializeField] private Text AxeReq2;
    [SerializeField] private Text PlankReq1;

    public List<string> inventoryItemList = new List<string>();

    public bool isOpen;

    [Header("Blueprints")]
    public BluePrint AxeBLP = new BluePrint("Axe", 1, 2, "Stone", 3, "Stick", 3);
    public BluePrint PlankBLP = new BluePrint("Plank", 2, 1, "Log", 1, "", 0);
    public BluePrint BlueprintBLP = new BluePrint("Blueprint", 1, 0, "", 0, "", 0);

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

    private void Start()
    {
        if (!isOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        isOpen = false;

        toolsBTN.onClick.AddListener(OpenToolsCategory);
        survivalBTN.onClick.AddListener(OpenSurvivalCategory);
        refineBTN.onClick.AddListener(OpenRefineCategory);

        craftAxeBTN.onClick.AddListener(() => CraftAnyItem(AxeBLP));
        craftPlankBTN.onClick.AddListener(() => CraftAnyItem(PlankBLP));
        craftBlueprintBTN.onClick.AddListener(() => CraftAnyItem(BlueprintBLP));
    }

    private void Update()
    {
        if (isOpen)
        {
            RefreshNeededItems();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isOpen)
            {
                OpenCrafting();
            }
            else
            {
                CloseCrafting();
            }
        }
    }

    public void OpenCrafting()
    {
        craftingScreenUI.SetActive(true);

        ToolsCategoryScreen.SetActive(true);
        SurvivalCategoryScreen.SetActive(false);
        RefineCategoryScreen.SetActive(false);

        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        isOpen = true;
    }

    public void CloseCrafting()
    {
        craftingScreenUI.SetActive(false);

        ToolsCategoryScreen.SetActive(false);
        SurvivalCategoryScreen.SetActive(false);
        RefineCategoryScreen.SetActive(false);

        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        isOpen = false;

        if (!InventorySystem.Instance.isOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }
    }

    private void OpenToolsCategory()
    {
        ToolsCategoryScreen.SetActive(false);
        SurvivalCategoryScreen.SetActive(false);
        RefineCategoryScreen.SetActive(false);

        toolsScreenUI.SetActive(true);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        RefreshNeededItems();
    }

    private void OpenSurvivalCategory()
    {
        ToolsCategoryScreen.SetActive(false);
        SurvivalCategoryScreen.SetActive(false);
        RefineCategoryScreen.SetActive(false);

        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(true);
        refineScreenUI.SetActive(false);

        RefreshNeededItems();
    }

    private void OpenRefineCategory()
    {
        ToolsCategoryScreen.SetActive(false);
        SurvivalCategoryScreen.SetActive(false);
        RefineCategoryScreen.SetActive(false);

        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(true);

        RefreshNeededItems();
    }

    private void CraftAnyItem(BluePrint bluePrintToCraft)
    {
        for (int i = 0; i < bluePrintToCraft.numberOfItemsProduce; i++)
        {
            InventorySystem.Instance.AddToInventory(bluePrintToCraft.itemName);
        }

        if (bluePrintToCraft.numbofRequirments >= 1)
        {
            RemoveItemFromInventoryAndQuickSlots(bluePrintToCraft.Req1, bluePrintToCraft.Req1amount);
        }

        if (bluePrintToCraft.numbofRequirments >= 2)
        {
            RemoveItemFromInventoryAndQuickSlots(bluePrintToCraft.Req2, bluePrintToCraft.Req2amount);
        }

        StartCoroutine(Calculate());
        RefreshNeededItems();
    }

    private void RemoveItemFromInventoryAndQuickSlots(string itemName, int amount)
    {
        int remaining = amount;

        //produce amount of items according to blueprint
        for (int i = InventorySystem.Instance.itemList.Count - 1; i >= 0; i--)
        {
            if (remaining <= 0)
            {
                break;
            }

            if (InventorySystem.Instance.itemList[i] == itemName)
            {
                InventorySystem.Instance.RemoveItem(itemName, 1);
                remaining--;
            }
        }

        if (remaining > 0)
        {
            foreach (GameObject slot in EquipSystem.Instance.quickSlotsList)
            {
                if (remaining <= 0)
                {
                    break;
                }

                if (slot.transform.childCount > 0)
                {
                    GameObject itemInSlot = slot.transform.GetChild(0).gameObject;
                    string cleanName = itemInSlot.name.Replace("(Clone)", "");

                    if (cleanName == itemName)
                    {
                        Destroy(itemInSlot);
                        remaining--;
                    }
                }
            }
        }

        InventorySystem.Instance.ReCalculateList();
    }

    private IEnumerator Calculate()
    {
        yield return new WaitForSeconds(1f);
        InventorySystem.Instance.ReCalculateList();
    }

    public void RefreshNeededItems()
    {
        if (InventorySystem.Instance == null)
        {
            Debug.LogError("InventorySystem.Instance is null");
            return;
        }

        if (InventorySystem.Instance.itemList == null)
        {
            Debug.LogError("InventorySystem.Instance.itemList is null");
            return;
        }

        if (AxeReq1 == null || AxeReq2 == null || PlankReq1 == null)
        {
            Debug.LogError("One or more requirement text references are null");
            return;
        }

        if (craftAxeBTN == null || craftPlankBTN == null)
        {
            Debug.LogError("One or more craft button references are null");
            return;
        }

        int stoneCount = 0;
        int stickCount = 0;
        int logCount = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            if (itemName == "Stone")
            {
                stoneCount++;
            }
            else if (itemName == "Stick")
            {
                stickCount++;
            }
            else if (itemName == "Log")
            {
                logCount++;
            }
        }

        foreach (GameObject slot in EquipSystem.Instance.quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                GameObject itemInSlot = slot.transform.GetChild(0).gameObject;
                string cleanName = itemInSlot.name.Replace("(Clone)", "");

                if (cleanName == "Stone")
                {
                    stoneCount++;
                }
                else if (cleanName == "Stick")
                {
                    stickCount++;
                }
                else if (cleanName == "Log")
                {
                    logCount++;
                }
            }
        }

        bool hasStone = stoneCount >= AxeBLP.Req1amount;
        bool hasStick = stickCount >= AxeBLP.Req2amount;
        bool hasLog = logCount >= PlankBLP.Req1amount;

        AxeReq1.text = AxeBLP.Req1amount + " " + AxeBLP.Req1 + " [" + stoneCount + "]";
        AxeReq2.text = AxeBLP.Req2amount + " " + AxeBLP.Req2 + " [" + stickCount + "]";
        PlankReq1.text = PlankBLP.Req1amount + " " + PlankBLP.Req1 + " [" + logCount + "]";

        AxeReq1.color = hasStone ? Color.green : Color.red;
        AxeReq2.color = hasStick ? Color.green : Color.red;
        PlankReq1.color = hasLog ? Color.green : Color.red;

        craftAxeBTN.interactable = hasStone && hasStick;
        craftPlankBTN.interactable = hasLog;
    }
}