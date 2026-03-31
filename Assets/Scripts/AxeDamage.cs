using UnityEngine;

public class AxeDamage : MonoBehaviour
{
    public float axeDamage = 2.5f;


    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other) 
    {
        AnimalHealth  enemy = other.GetComponent<AnimalHealth >();
        if(enemy != null)
        {
            
            enemy.TakeDamage(axeDamage);

        }
    }
}
