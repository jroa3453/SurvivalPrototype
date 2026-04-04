using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class EquippableItem : MonoBehaviour
{
    public Animator animator;
    public float axeDamage = 10f;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) 
            && CraftingSystem.Instance.isOpen == false
            && InventorySystem.Instance.isOpen == false
            && !Campfire.Instance.playerInRange
            && !SaveLoadUI.Instance.isOpen)
        { 
            if (animator.runtimeAnimatorController != null)
            {
                animator.Play("Axe_Hit");
                StartCoroutine(DelayedHit());
            }
        }  
    }

    IEnumerator DelayedHit()
    {
        yield return new WaitForSeconds(0.2f);
        GetHit();
    }
    
    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.SelectedTree;
        if(selectedTree != null)
        {
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }

        GameObject selectedAnimal = SelectionManager.Instance.selectedObject;
        if(selectedAnimal != null)
        {
            AnimalHealth animal = selectedAnimal.GetComponent<AnimalHealth>();
            if(animal != null)
            {
                animal.TakeDamage(axeDamage);
            }
        }
    }
}