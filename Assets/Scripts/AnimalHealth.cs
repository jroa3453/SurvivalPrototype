using UnityEngine;

public class AnimalHealth : MonoBehaviour
{
    public float MaxHealth;
    public float currentHealth; 
    public GameObject meatPreFab;
   
    void Start()
    {
        currentHealth = MaxHealth;
    }

   
    void Update()
    {

    }

    public void TakeDamage(float Damage)
    {
        currentHealth -= Damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(meatPreFab, transform.position, Quaternion.identity);
                Destroy(gameObject);
    }




}
