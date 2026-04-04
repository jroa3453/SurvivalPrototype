using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    public int daysSurvived = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.slotToLoad != -1)
        {
            LoadGame(GameManager.Instance.slotToLoad);
            GameManager.Instance.slotToLoad = -1;
        }
    }

    public void SaveGame(int slot)
    {
        SaveData data = new SaveData();

        data.saveDate = System.DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        data.daysSurvived = daysSurvived;

        Vector3 pos = GameObject.FindWithTag("Player").transform.position;
        data.playerX = pos.x;
        data.playerY = pos.y;
        data.playerZ = pos.z;

        data.currentHealth = PlayerState.Instance.currentHealth;
        data.currentCalories = PlayerState.Instance.currentCalories;
        data.currentHydration = PlayerState.Instance.currentHydrationPercent;

        data.currentTime = DayNightCycle.Instance.currentTime;

        data.inventoryItems = new System.Collections.Generic.List<string>(InventorySystem.Instance.itemList);
        data.quickSlotItems = new System.Collections.Generic.List<string>(EquipSystem.Instance.itemList);

        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "/save_slot_" + slot + ".json";
        File.WriteAllText(path, json);

        Debug.Log("Game saved to slot " + slot + " at " + path);
    }

    public void LoadGame(int slot)
    {
        string path = Application.persistentDataPath + "/save_slot_" + slot + ".json";

        if (!File.Exists(path))
        {
            Debug.Log("No save file found in slot " + slot);
            return;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        Vector3 pos = new Vector3(data.playerX, data.playerY, data.playerZ);
        GameObject.FindWithTag("Player").transform.position = pos;

        PlayerState.Instance.SetHealth(data.currentHealth);
        PlayerState.Instance.SetCalories(data.currentCalories);
        PlayerState.Instance.SetHydration(data.currentHydration);

        DayNightCycle.Instance.currentTime = data.currentTime;

        InventorySystem.Instance.itemList.Clear();
        foreach (string item in data.inventoryItems)
            InventorySystem.Instance.AddToInventory(item);

        Debug.Log("Game loaded from slot " + slot);
    }

    public SaveData GetSlotData(int slot)
    {
        string path = Application.persistentDataPath + "/save_slot_" + slot + ".json";
        if (!File.Exists(path)) return null;
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public void DeleteSave(int slot)
    {
        string path = Application.persistentDataPath + "/save_slot_" + slot + ".json";
        if (File.Exists(path)) File.Delete(path);
        Debug.Log("Deleted save slot " + slot);
    }
}