using UnityEngine;
using System.Collections;


public class ChoppableTree : MonoBehaviour
{
    public bool playerInRange;
    public bool canBeChopped;
    public float treeMaxHealth;
    public float treeHealth;

    public float CaloriesSpentChoppingWood = 15f;

    private TreeisChopped treeisChoppedScript;

    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            canBeChopped = false;
        }
    }

    void Start()
{
    treeHealth = treeMaxHealth;
    
    // Get Animator on THIS object since ChoppableTree is now on Tree_Parent
    animator = GetComponent<Animator>();
    
    // If not found on this object, try the parent
    if (animator == null)
        animator = GetComponentInParent<Animator>();
        
    if (animator == null)
        Debug.LogError("No Animator found on " + gameObject.name);

    treeisChoppedScript = GetComponentInParent<TreeisChopped>();
    Debug.Log("Found TreeisChopped: " + treeisChoppedScript);
}

    public void GetHit()
    {
        animator.SetTrigger("shake");

        treeHealth -= 10f;
        treeHealth = Mathf.Clamp(treeHealth, 0f, treeMaxHealth);

        PlayerState.Instance.currentCalories -= CaloriesSpentChoppingWood;
        GlobalState.Instance.resourceHealth = treeHealth;
        GlobalState.Instance.resourceMaxHealth = treeMaxHealth;

        Debug.Log("Tree health now: " + treeHealth);

        if (treeHealth <= 0f)
        {
            Debug.Log("Tree has been chopped down");
            
            TreeisDead();
        

            if (treeisChoppedScript != null)
            {
                treeisChoppedScript.ChopTreeDown();
            }
            else
            {
                Debug.Log("TreeisChopped script is NULL");
            }

            gameObject.SetActive(false);
        }
    }

    void TreeisDead()
{
    Vector3 treePosition = transform.position;

    canBeChopped = false;

    SelectionManager.Instance.SelectedTree = null;
    SelectionManager.Instance.chopHolder.SetActive(false);

    GlobalState.Instance.resourceHealth    = 0f;
    GlobalState.Instance.resourceMaxHealth = 0f;

    // Spawn the chopped tree stump
    Instantiate(Resources.Load<GameObject>("ChoppedTree"), 
        new Vector3(treePosition.x, treePosition.y, treePosition.z), 
        Quaternion.identity);

    // Destroy THIS object (Tree_Parent) — not parents above it
    Destroy(gameObject);
}



}