using UnityEngine;
using TMPro;

public class QuestTrackerUI : MonoBehaviour
{
    public static QuestTrackerUI Instance {get; set; }
    public GameObject questTrackerPanel;
    public TMP_Text questTitleText;
    public TMP_Text objectiveText;



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
    private void Start()
    {
        questTrackerPanel.SetActive(false);
    }

    private void Update()
    {
        if (QuestSystem.Instance.activeQuests.Count > 0)
        {
            questTrackerPanel.SetActive(true);
            Quest currentQuest = QuestSystem.Instance.activeQuests[0];
            questTitleText.text = currentQuest.questName;

            foreach (QuestObjective objective in currentQuest.objectives)
            {
                if (!objective.isCompleted)
                {
                    objectiveText.text = objective.objectiveDescription;
                    break;
                }
            }
        }
        else
        {
            questTrackerPanel.SetActive(false);
        }
    }
}
