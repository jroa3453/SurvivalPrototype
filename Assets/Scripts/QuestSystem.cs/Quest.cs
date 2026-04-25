using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    public string questDescription;
    public List<QuestObjective> objectives;
    public bool isActive;
    public bool isCompleted;
    public string rewardItem;
    public int rewardAmount;
}

[System.Serializable]
public class QuestObjective
{
    public string objectiveDescription;
    public bool isCompleted;
    public int requiredAmount;
    public int currentAmount;
}