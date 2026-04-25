using System.Reflection;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class EquippableItem : MonoBehaviour
{
    public Animator animator;
    
    [Header("Combat")]
    public float attackCooldown = 1f; // time between attacks
    [Header("Damage")]
    [Header("Animation")]
    public int damage;
    public bool canHitAnimals;
    public bool canChopTrees;
    private bool canAttack = true;
    public string hitAnimationName; // one value for all weapons, set per prefab in Inspector

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
          
        }
        if (Input.GetMouseButtonDown(0)
            && CraftingSystem.Instance.isOpen == false
            && InventorySystem.Instance.isOpen == false
            && !Campfire.Instance.playerInRange
            && !SaveLoadUI.Instance.isOpen)
        {
             if(animator.runtimeAnimatorController != null)
            {
                animator.Play(hitAnimationName);
                StartCoroutine(AttackCooldown());
            } 
        }
    }


    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Called by animation event
    public void GetHit()
    {
        // Tree chopping — axe only
        if (gameObject.name.ToLower().Contains("axe"))
        {
            GameObject selectedTree = SelectionManager.Instance.SelectedTree;
            if (selectedTree != null)
            {
                ChoppableTree tree = selectedTree.GetComponentInParent<ChoppableTree>();
                if (tree != null) tree.GetHit();
            }
        }

        // Animal damage — any weapon
        GameObject selectedAnimal = SelectionManager.Instance.selectedObject;
        if (selectedAnimal != null)
        {
            Animal animal = selectedAnimal.GetComponent<Animal>();
            if (animal != null)
                animal.TakeDamage(damage);
        }
    }
}