using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class EquippableItem : MonoBehaviour
{
    public Animator animator;
    public float axeDamage = 10f;
    public float hammerDamage = 5.5f;
    
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
                string itemName = EquipSystem.Instance.selectedItem != null ? 
                    EquipSystem.Instance.selectedItem.name.Replace("(Clone)", "").Trim() : "";
                    Debug.Log("Item name: " + itemName);
                    Debug.Log("GameObject name: " + gameObject.name);
                    Debug.Log("Parent name: " + transform.parent.name);

                if (gameObject.name.Contains("Hammer"))
                    animator.SetTrigger("Hit");
                else
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
        // Only chop trees if holding axe
        if (gameObject.name.Contains("Axe"))
        {
            GameObject selectedTree = SelectionManager.Instance.SelectedTree;
            if(selectedTree != null)
            {
                selectedTree.GetComponent<ChoppableTree>().GetHit();
            }
        }

        // Only damage animals if holding axe
        if (gameObject.name.Contains("Axe"))
        {
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
}