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
    public AudioSource chopSound;

    //Audio



    
    public AudioSource TreeFallingSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            canBeChopped = true;
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

        // ✅ Just play the chop sound directly
        if (chopSound != null)
            chopSound.Play();
        else
            Debug.LogWarning("chopSound is not assigned on " + gameObject.name);

        if (treeHealth <= 0f)
        {
            Debug.Log("Tree has been chopped down");
            TreeFallingSound.Play();
            TreeisDead();

            if (treeisChoppedScript != null)
                treeisChoppedScript.ChopTreeDown();
            else
                Debug.Log("TreeisChopped script is NULL");
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