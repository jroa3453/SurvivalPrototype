using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestLogUI : MonoBehaviour
{
   public static QuestLogUI Instance {get; set; }
   public GameObject questLogPanel;
   public Transform activeQuestContent;
   public Transform completedQuestContent;
   private bool isOpen = false;



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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isOpen = !isOpen;
            questLogPanel.SetActive(isOpen);
            RefreshQuestLog();
        }
    }

    public void RefreshQuestLog()
    {
        foreach (Transform child in activeQuestContent)
            Destroy(child.gameObject);

        foreach (Transform child in completedQuestContent)
            Destroy(child.gameObject);

        foreach (Quest quest in QuestSystem.Instance.activeQuests)
        {
           GameObject entry = Instantiate(Resources.Load<GameObject>("QuestEntry"), activeQuestContent);
           entry.GetComponent<TMP_Text>().text = quest.questName;
        }

        foreach (Quest quest in QuestSystem.Instance.activeQuests)
        {
           GameObject entry = Instantiate(Resources.Load<GameObject>("QuestEntry"), completedQuestContent);
           entry.GetComponent<TMP_Text>().text = quest.questName;
        }
    }
}
