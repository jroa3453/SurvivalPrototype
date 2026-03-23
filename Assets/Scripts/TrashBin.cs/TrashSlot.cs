using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    public GameObject trashAlertUI;
    public Text promptText;
    public Button yesButton;
    public Button noButton;
    public Image trashIconImage;

    [Header("Trash Bin Sprites")]
    public Sprite trashClosed;
    public Sprite trashOpened;

    private GameObject itemToBeDeleted;
    private bool isPointerOverTrash = false;

    private GameObject DraggedItem
    {
        get { return DragDrop.itemBeingDragged; }
    }

    private void Start()
    {
        if (trashAlertUI != null)
            trashAlertUI.SetActive(false);

        if (yesButton != null)
            yesButton.onClick.AddListener(DeleteItem);

        if (noButton != null)
            noButton.onClick.AddListener(CancelDeletion);

        SetClosedSprite();
    }

    private void Update()
    {
        if (isPointerOverTrash && DraggedItem != null)
        {
            InventoryItem inventoryItem = DraggedItem.GetComponent<InventoryItem>();
            if (inventoryItem != null && inventoryItem.isTrashable)
            {
                SetOpenSprite();
                return;
            }
        }

        SetClosedSprite();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DraggedItem == null)
            return;

        InventoryItem inventoryItem = DraggedItem.GetComponent<InventoryItem>();
        if (inventoryItem == null || !inventoryItem.isTrashable)
            return;

        itemToBeDeleted = DraggedItem;
        ShowDeletePrompt();

        DragDrop.itemBeingDragged = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOverTrash = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOverTrash = false;
    }

    private void ShowDeletePrompt()
    {
        if (trashAlertUI != null)
            trashAlertUI.SetActive(true);

        if (promptText != null)
            promptText.text = "Throw away this " + GetCleanItemName() + "?";
    }

    private string GetCleanItemName()
    {
        if (itemToBeDeleted == null)
            return "item";

        return itemToBeDeleted.name.Replace("(Clone)", "").Trim();
    }

    private void CancelDeletion()
    {
        SetClosedSprite();

        if (trashAlertUI != null)
            trashAlertUI.SetActive(false);

        itemToBeDeleted = null;
    }

    private void DeleteItem()
    {
        if (itemToBeDeleted != null)
        {
            Destroy(itemToBeDeleted);

            if (InventorySystem.Instance != null)
                InventorySystem.Instance.ReCalculateList();

            if (CraftingSystem.Instance != null)
                CraftingSystem.Instance.RefreshNeededItems();
        }

        SetClosedSprite();

        if (trashAlertUI != null)
            trashAlertUI.SetActive(false);

        itemToBeDeleted = null;
    }

    private void SetClosedSprite()
    {
        if (trashIconImage != null && trashClosed != null)
            trashIconImage.sprite = trashClosed;
    }

    private void SetOpenSprite()
    {
        if (trashIconImage != null && trashOpened != null)
            trashIconImage.sprite = trashOpened;
    }
}