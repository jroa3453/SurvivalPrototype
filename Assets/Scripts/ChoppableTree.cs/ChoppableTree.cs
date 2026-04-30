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

        // ← Add delay to match animation
        StartCoroutine(PlayChopSound(0.3f));

        if (treeHealth <= 0f)
        {
            TreeFallingSound.Play();
            TreeisDead();
        }
    }

    IEnumerator PlayChopSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (chopSound != null)
            chopSound.Play();
    }

    void TreeisDead()
    {
        Vector3 treePosition = transform.position;

        canBeChopped = false;

        SelectionManager.Instance.SelectedTree = null;
        SelectionManager.Instance.chopHolder.SetActive(false);

        GlobalState.Instance.resourceHealth    = 0f;
        GlobalState.Instance.resourceMaxHealth = 0f;

        // ── Stump ────────────────────────────────────────────────────────────
        GameObject stumpPrefab = Resources.Load<GameObject>("ChoppedTree");
        if (stumpPrefab == null)
            Debug.LogError("[Tree] Resources.Load failed: 'ChoppedTree' not found in Resources/");
        else
            Instantiate(stumpPrefab, treePosition, Quaternion.identity);

        // ── Logs ─────────────────────────────────────────────────────────────
        GameObject logPrefab = Resources.Load<GameObject>("Log_Model");
        if (logPrefab == null)
            Debug.LogError("[Tree] Resources.Load failed: 'Log_Model' not found in Resources/");
        else
        {
            for (int i = 0; i < 2; i++)
            {
                Vector3 spawnPos = new Vector3(
                    treePosition.x + Random.Range(-1f, 1f),
                    treePosition.y + 1.5f,   // raised higher — avoids terrain clip
                    treePosition.z + Random.Range(-1f, 1f));
                Instantiate(logPrefab, spawnPos, Quaternion.identity);
            }
        }

        // ── Sticks ───────────────────────────────────────────────────────────
        GameObject stickPrefab = Resources.Load<GameObject>("Stick_Model");
        if (stickPrefab == null)
            Debug.LogError("[Tree] Resources.Load failed: 'Stick_Model' not found in Resources/");
        else
        {
            for (int i = 0; i < 2; i++)
            {
                Vector3 spawnPos = new Vector3(
                    treePosition.x + Random.Range(-1f, 1f),
                    treePosition.y + 1.5f,
                    treePosition.z + Random.Range(-1f, 1f));
                Instantiate(stickPrefab, spawnPos, Quaternion.identity);
            }
        }

        // ── Delay destroy so TreeFallingSound isn't cut off ──────────────────
        Destroy(gameObject, 0.1f);
    }
}