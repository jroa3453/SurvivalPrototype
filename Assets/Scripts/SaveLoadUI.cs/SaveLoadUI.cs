using UnityEngine;
using UnityEngine.UI;

public class SaveLoadUI : MonoBehaviour
{
    public static SaveLoadUI Instance;

    public GameObject saveLoadPanel;
    public GameObject confirmPanel;
    public Button[] slotButtons;
    public Text[] slotTexts;
    public Text confirmText;
    public Text CancelText;
    private int slotToDelete;
    public Button[] deleteButtons;

    public bool isOpen = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Start()
    {
        saveLoadPanel.SetActive(false);
        RefreshSlots();
    }

    public void OpenSaveLoad()
    {
        saveLoadPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isOpen = true;
        RefreshSlots();
    }

    public void CloseSaveLoad()
    {
        saveLoadPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isOpen = false;
        Time.timeScale = 1f;
    }

    void RefreshSlots()
    {
        for (int i = 0; i < 5; i++)
        {
            SaveData data = SaveSystem.Instance.GetSlotData(i + 1);
            if (data != null)
            {
                slotTexts[i].text = "Slot " + (i + 1) + "\n" + data.saveDate + "\nDay " + data.daysSurvived;
                deleteButtons[i].gameObject.SetActive(true);
            }
            else
            {
                slotTexts[i].text = "Slot " + (i + 1) + "\n--- Empty ---";
                deleteButtons[i].gameObject.SetActive(true);
            }
        }
    }

    public void OnSaveSlot(int slot)
    {
        Debug.Log("Saving to slot " + slot);
        SaveSystem.Instance.SaveGame(slot);
        RefreshSlots();
    }

    public void OnLoadSlot(int slot)
    {
        SaveSystem.Instance.LoadGame(slot);
        CloseSaveLoad();
    }

    public void OnDeleteSlot(int slot)
    {
        slotToDelete = slot;
        confirmText.text = "Are you sure you want to delete the save in slot " + slot + "?";
        confirmPanel.SetActive(true);
    }
    public void ConfirmDelete()
    {
        SaveSystem.Instance.DeleteSave(slotToDelete);
        confirmPanel.SetActive(false);
        RefreshSlots();
    }

    public void CancelDelete()
    {
        confirmPanel.SetActive(false);
    }
}