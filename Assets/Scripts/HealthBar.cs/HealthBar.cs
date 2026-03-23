using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{

    public Text healthCounter;

    private Slider slider;

    public GameObject playerState;

    private float currentHealth;
    private float maxHealth;

    void Awake()
    {
       slider = GetComponent<Slider>();
    }

    
    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;
        healthCounter.text = currentHealth + " / " + maxHealth;

    }
}
