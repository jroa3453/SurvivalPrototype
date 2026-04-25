using UnityEngine;
using System.Collections.Generic;

public class QuestSystem : MonoBehaviour
{
    public static QuestSystem Instance {get; set; }
    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();  

    public void Awake()
    {
        
        if(Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    

    public void AddQuest(Quest quest)
    {
        quest.isActive = true;
        activeQuests.Add(quest);
    }

    public void CompletedQuest(Quest quest)
    {
        quest.isActive = false;
        quest.isCompleted = true;
        activeQuests.Remove(quest);
        completedQuests.Add(quest);
    }

    public void UpdateObjective(string questname, string objectiveDescription)
    {
        foreach(Quest quest in activeQuests)
        {
            if (quest.questName == questname)
            {
                foreach (QuestObjective objective in quest.objectives)
                {
                    if (objective.objectiveDescription == objectiveDescription)
                    {
                        objective.currentAmount++;
                        if(objective.currentAmount >= objective.requiredAmount)
                                objective.isCompleted = true;
                    }
                }
            }
        }
    }
}
