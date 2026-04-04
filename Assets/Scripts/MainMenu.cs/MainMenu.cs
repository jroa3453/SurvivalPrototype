using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject loadGamePanel;

    public void NewGame()
    {
        SceneManager.LoadScene("MainSceneProtoTypeSurvival.Unity");
    }

    public void LoadGame()
    {
        loadGamePanel.SetActive(true);
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