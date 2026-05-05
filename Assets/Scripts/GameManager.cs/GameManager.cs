using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int slotToLoad = -1;
<<<<<<< HEAD
=======
    public bool isNewGame = false;
>>>>>>> 0a5989b6fd4b22784c4c20e3b41f614aac0069e4

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}