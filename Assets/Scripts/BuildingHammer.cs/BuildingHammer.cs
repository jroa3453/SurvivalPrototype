using UnityEngine;

public class BuildingHammer : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (BuildingSystem.Instance.inBuildMode)
            {
                BuildingSystem.Instance.ExitBuildMode();
            }
            else
            {
                if (InventorySystem.Instance.itemList.Contains("Blueprint"))
                {
                    BuildingSystem.Instance.EnterBuildMode();
                }
                else
                {
                    Debug.Log("You need a Blueprint to build!");
                }
            }
        }
    }
}