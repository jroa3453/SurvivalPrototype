using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

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
    }
    
    public void GetHit()
    {
         GameObject selectedTree = SelectionManager.Instance.SelectedTree;

            if(selectedTree != null)
            {
                selectedTree.GetComponent<ChoppableTree>().GetHit();
            }
    }
   
   




}
