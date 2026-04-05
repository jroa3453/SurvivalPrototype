using UnityEngine;

public class BuildingHammer : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (BuildingSystem.Instance.inBuildMode)
                BuildingSystem.Instance.ExitBuildMode();
            else
                BuildingSystem.Instance.EnterBuildMode();
        }
    }
}