using UnityEngine;
using UnityEngine.AI;


public class AnimalHealth : MonoBehaviour
{
    public float MaxHealth;
    public float currentHealth; 
    public GameObject meatPreFab;
    private NavMeshAgent agent;
   
    void Start()
    {
        currentHealth = MaxHealth;
        agent = GetComponent<NavMeshAgent>();
    }

   
    void Update()
    {

    }

    public void TakeDamage(float Damage)
    {
        currentHealth -= Damage;
         Flee();
        if(currentHealth <= 0)
        {
            Die();
        }
    }


    void Flee()
    {
        Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
        Vector3 fleeDirection = transform.position - playerPosition;
        Vector3 fleePosition = transform.position + fleeDirection;
        agent.SetDestination(fleePosition);
    }

    void Die()
    {
        Instantiate(meatPreFab, transform.position, Quaternion.identity);
                Destroy(gameObject);
    }
}
