using UnityEngine;

public class ToolAttack : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left click
        {
            animator.SetTrigger("Attack");
        }
    }
}