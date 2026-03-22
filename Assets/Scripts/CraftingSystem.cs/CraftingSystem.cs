using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{

    public static CraftingSystem Instance { get; set; }

    [SerializeField] private GameObject craftingScreenUI;
    [SerializeField] private GameObject toolsScreenUI;

    public List<string> inventoryItemList = new List<string>();

    //Category Buttons
    [SerializeField] private Button toolsBTN;

    //Craft Buttons
    [SerializeField] private Button craftAxeBTN;

    //Requirment Text
   [SerializeField] private Text AxeReq1;
   [SerializeField] private Text AxeReq2;
   [SerializeField] private GameObject ToolsCategoryScreen;

    public bool isOpen;

    //All BluePrints

    public BluePrint AxeBLP = new BluePrint("Axe", 2, "Stone", 3, "Stick", 3);


    void Awake()
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if(!CraftingSystem.Instance.isOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        isOpen = false;
        craftAxeBTN.onClick.AddListener(delegate {CraftAnyItem(AxeBLP);});
        toolsBTN.onClick.AddListener(OpenToolsCategory);

    }


void CraftAnyItem(BluePrint bluePrintToCraft)
{
    InventorySystem.Instance.AddToInventory(bluePrintToCraft.itemName);

    if (bluePrintToCraft.numbofRequirments >= 1)
    {
        InventorySystem.Instance.RemoveItem(bluePrintToCraft.Req1, bluePrintToCraft.Req1amount);
    }

    if (bluePrintToCraft.numbofRequirments >= 2)
    {
        InventorySystem.Instance.RemoveItem(bluePrintToCraft.Req2, bluePrintToCraft.Req2amount);
    }

    StartCoroutine(calculate());
    
    RefreshNeededItems();
}


void OpenToolsCategory()
    {
        ToolsCategoryScreen.SetActive(false);

        toolsScreenUI.SetActive(true);

        RefreshNeededItems();
    }




    public IEnumerator calculate()
    {
        yield return new WaitForSeconds(1f);

        InventorySystem.Instance.ReCalculateList();
    }

    // Update is called once per frame
void Update()
{
    if (isOpen && toolsScreenUI.activeSelf)
    {
        RefreshNeededItems();
    }

    if (Input.GetKeyDown(KeyCode.Tab) && !isOpen)
    {
        craftingScreenUI.SetActive(true);
        ToolsCategoryScreen.SetActive(true);
        toolsScreenUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        isOpen = true;
    }
    else if (Input.GetKeyDown(KeyCode.Tab) && isOpen)
    {
        craftingScreenUI.SetActive(false);
        ToolsCategoryScreen.SetActive(false);
        toolsScreenUI.SetActive(false);

        if (!InventorySystem.Instance.isOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        isOpen = false;
    }
}

private void RefreshNeededItems()
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

    if (AxeReq1 == null)
    {
        Debug.LogError("AxeReq1 is null");
        return;
    }

    if (AxeReq2 == null)
    {
        Debug.LogError("AxeReq2 is null");
        return;
    }

    if (craftAxeBTN == null)
    {
        Debug.LogError("craftAxeBTN is null");
        return;
    }

    int stone_count = 0;
    int stick_count = 0;

    inventoryItemList = InventorySystem.Instance.itemList;

    foreach (string itemName in inventoryItemList)
    {
        if (itemName == "Stone")
        {
            stone_count++;
        }
        else if (itemName == "Stick")
        {
            stick_count++;
        }
    }

    AxeReq1.text = AxeBLP.Req1amount + " " + AxeBLP.Req1 + " [" + stone_count + "]";
    AxeReq2.text = AxeBLP.Req2amount + " " + AxeBLP.Req2 + " [" + stick_count + "]";

    craftAxeBTN.interactable = stone_count >= AxeBLP.Req1amount && stick_count >= AxeBLP.Req2amount;

    Debug.Log("Req1 text is now: " + AxeReq1.text);
    Debug.Log("Req2 text is now: " + AxeReq2.text);

    bool hasReq1 = stone_count >= AxeBLP.Req1amount;
    bool hasReq2 = stick_count >= AxeBLP.Req2amount;
    bool hasAllRequirements = stone_count >= AxeBLP.Req1amount && 
                            stick_count >= AxeBLP.Req2amount;

    AxeReq1.color = hasReq1 ? Color.green : Color.red;
    AxeReq2.color = hasReq2 ? Color.green : Color.red;

        if (hasReq1 && hasReq2)
         {
             craftAxeBTN.interactable = true;
         }
         else
          {
             craftAxeBTN.interactable = false;
          }
        {
            craftAxeBTN.gameObject.SetActive(hasAllRequirements);
        }
 }
}
