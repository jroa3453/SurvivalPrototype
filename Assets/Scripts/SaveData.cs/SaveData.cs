using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public string saveDate;
    public int daysSurvived;

    public float playerX;
    public float playerY;
    public float playerZ;

    public float currentHealth;
    public float currentCalories;
    public float currentHydration;

    public float currentTime;

    public List<string> inventoryItems = new List<string>();
    public List<string> quickSlotItems = new List<string>();
}