using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquippableItem : MonoBehaviour
{


    public Animator animator;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) 
            && CraftingSystem.Instance.isOpen == false
            && InventorySystem.Instance.isOpen == false)
        { 
            // Guard check — only play if controller is actually assigned
            if (animator.runtimeAnimatorController != null)
            {
                animator.Play("Axe_Hit");
            }
        }  
    }
    
    public void GetHit()
    {
         GameObject selectedTree = SelectionManager.Instance.SelectedTree;

            if(selectedTree != null)
            {
                selectedTree.GetComponentInParent<ChoppableTree>().GetHit();
            }
    }
}
