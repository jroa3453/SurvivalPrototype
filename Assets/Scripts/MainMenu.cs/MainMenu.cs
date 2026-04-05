using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject loadGamePanel;
    public TextMeshProUGUI[] slotTexts;

    void Start()
    {
        RefreshSlots();
    }

    void RefreshSlots()
    {
        Debug.Log("Save path: " + Application.persistentDataPath);
        for (int i = 0; i < 5; i++)
        {
            string path = Application.persistentDataPath + "/save_slot_" + (i + 1) + ".json";
            Debug.Log("Checking slot " + (i + 1) + " at: " + path + " | Exists: " + System.IO.File.Exists(path));
            if (System.IO.File.Exists(path))
            {
                string json = System.IO.File.ReadAllText(path);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                slotTexts[i].text = "Slot" + (i + 1) + "\n" + data.saveDate + "\nDay " + data.daysSurvived;
            }
            else
            {
                
                slotTexts[i].text = "Slot " + (i + 1) + "\n--- Empty ---";
                
            }
        }
    }





    public void NewGame()
    {
        SceneManager.LoadScene("MainSceneProtoTypeSurvival.Unity");
    }

    public void LoadGame()
    {
        loadGamePanel.SetActive(true);
    }

    public void LoadSlot(int slot)
    {
        string path = Application.persistentDataPath + "/save_slot_" + slot + ".json";
        
        if (!System.IO.File.Exists(path))
        {
            Debug.Log("No save data in slot " + slot);
            return;
        }
        
        GameManager.Instance.slotToLoad = slot;
        SceneManager.LoadScene("MainSceneProtoTypeSurvival.Unity");
    }

    public void CloseLoadGame()
    {
        loadGamePanel.SetActive(false);
    }

    public void Settings()
    {
        Debug.Log("Settings coming soon!");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }
}