using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public GameObject pauseMenuPanel;
    public GameObject saveLoadPanel;
    public bool isPaused = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void OpenPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }

    public void ClosePauseMenu()
    {
        pauseMenuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    public void Resume()
    {
        ClosePauseMenu();
    }

    public void OpenSaveLoad()
    {
        ClosePauseMenu();
        saveLoadPanel.SetActive(true);
        SaveLoadUI.Instance.isOpen = true;
    }

    public void Settings()
    {
        Debug.Log("Settings coming soon!");
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}