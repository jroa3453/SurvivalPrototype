using UnityEngine;

public class TreeisChopped : MonoBehaviour
{
    public GameObject treeStump;
    public GameObject standingTree;
    public GameObject logPrefab;
    public Transform logSpawnPoint1;
    public Transform logSpawnPoint2;

    private bool hasBeenChopped = false;

    void Start()
    {
        treeStump.SetActive(false);
    }

    public void ChopTreeDown()
    {
        Debug.Log("ChopTreeDown called");

        if (hasBeenChopped) return;

        hasBeenChopped = true;

        standingTree.SetActive(false);
        treeStump.SetActive(true);

        Debug.Log("Tree has been chopped down, spawning logs...");

        Instantiate(logPrefab, logSpawnPoint1.position + Vector3.up * 3f, Quaternion.identity);
        Instantiate(logPrefab, logSpawnPoint2.position + Vector3.up * 3f, Quaternion.identity);

        Debug.Log("Spawning log at: " + logSpawnPoint1.position);
        Debug.Log("Spawning log at: " + logSpawnPoint2.position);



        Debug.Log("Logs have been spawned.");
    }
}