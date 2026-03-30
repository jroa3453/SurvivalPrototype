using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject trashAlertUI;
    public Text messageText;

    public Button yesButton;
    public Button noButton;

    public Sprite trash_closed;
    public Sprite trash_opened;

    private Image trashCanImage;

    GameObject itemToBeDeleted;

    GameObject draggedItem
    {
        get { return DragDrop.itemBeingDragged; }
    }

    void Start()
    {
        // Get the trash can's own image
        trashCanImage = transform.Find("background").GetComponent<Image>();

        yesButton.onClick.AddListener(DeleteItem);
        noButton.onClick.AddListener(CancelDeletion);

        trashCanImage.sprite = trash_closed;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable)
        {
            itemToBeDeleted = draggedItem;
            trashAlertUI.SetActive(true);
            messageText.text = "Throw away this item?";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable)
        {
            trashCanImage.sprite = trash_opened;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        trashCanImage.sprite = trash_closed;
    }

    void DeleteItem()
    {
        if (itemToBeDeleted != null)
        {
            Destroy(itemToBeDeleted);
        }

        trashCanImage.sprite = trash_closed;
        trashAlertUI.SetActive(false);
    }

    void CancelDeletion()
    {
        trashCanImage.sprite = trash_closed;
        trashAlertUI.SetActive(false);
    }
}